var G_FACTOR_HIGH = 3000;
var G_FACTOR_LOW = 720;

var PLAYER_SIZE = 3;
var END_SIZE = 10;
var BLACK_HOLE_SIZE = 12;

var game = {
	width: 240,
	height: 160,

	ratio: null,
	current_width: null,
	current_height: null,
	canvas: null,
	ctx: null,

	is_finished: false,

	g_factor: G_FACTOR_LOW,
	clicked: false,
	camera: {
		x: 0,
		y: 0,

		update: function() {
			this.x = game.player.pos_x - game.width / 2;
			this.y = game.player.pos_y - game.height / 2;
		}
	},

	player: {
		pos_x: 0,
		pos_y: 90,
		vel_x: 100,
		vel_y: 0,
		size: PLAYER_SIZE,
		color: "#999",

		update: function(dt) {
			var acc_x = 0;
			var acc_y = 0;
			for (var i = 0; i < game.black_holes.length; i++) {
				acc_x += game.black_holes[i].force_x(this.pos_x, this.pos_y);
				acc_y += game.black_holes[i].force_y(this.pos_x, this.pos_y);
			}
			this.vel_x += acc_x * dt;
			this.vel_y += acc_y * dt;
			this.pos_x += this.vel_x * dt;
			this.pos_y += this.vel_y * dt;

			game.particles.push(new game.make.player_particle(this.pos_x, this.pos_y));
		},

		render: function(camera) {
		 	game.draw.circle(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.color);
		}
	},

	end: {
		pos_x: 150,
		pos_y: 120,
		size: END_SIZE,
		color: null,
		counter: 3,
		up: true,

		test_finish: function(player_x, player_y) {

		},

		update: function(dt) {
			var step = 4 * dt;
			this.counter += (this.up) ? step : -step;
			if (this.counter > 5) this.up = false;
			if (this.counter < 3) this.up = true;
		},

		render: function(camera) {
			this.color = game.ctx.createRadialGradient(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.pos_x - camera.x, this.pos_y - camera.y, this.size-this.counter);
			this.color.addColorStop(0,"blue");
			this.color.addColorStop(1,"black");
			game.draw.circle(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.color)
		}
	},

	black_holes: [],

	stars: [],

	particles: [],

	init: function() {
		game.canvas = document.getElementById("game");

		game.ctx = game.canvas.getContext("2d");

		game.resize();

		game.load_scene(scene1);
	},

	resize: function() {
		game.current_width = window.innerWidth;
		game.current_height = window.innerHeight;
		game.ratio = game.current_height / game.current_width;
		game.width = 320;
		game.height = game.width * game.ratio;
		game.canvas.width = game.width;
		game.canvas.height = game.height;
	},

	draw: {
		clear: function() {
			game.ctx.clearRect(0, 0, game.width, game.height);
		},

		rect: function(x, y, w, h, color) {
			game.ctx.fillStyle = color;
			game.ctx.fillRect(x, y, w, h);
		}, 

		circle: function(x, y, r, color) {
			game.ctx.fillStyle = color;
			game.ctx.beginPath();
			game.ctx.arc(x, y, r, 0, Math.PI * 2, true);
			game.ctx.closePath();
			game.ctx.fill();
		},

		text: function(string, x, y, size, color) {
			game.ctx.fillStyle = color;
			game.ctx.font = "bold " + size + "px Monospace";
			game.ctx.fillText(string, x, y)
		}
	},

	load_scene: function(scene) {
		game.is_finished = false;
		game.black_holes = [];
		game.stars = [];
		game.particles = [];

		game.player.pos_x = scene.player.pos_x;
		game.player.pos_y = scene.player.pos_y;
		game.player.vel_x = scene.player.vel_x;
		game.player.vel_y = scene.player.vel_y;

		for (var i = 0; i < scene.black_holes.length; i++) {
			var x = scene.black_holes[i].pos_x;
			var y = scene.black_holes[i].pos_y;
			game.black_holes.push(new game.make.black_hole(x, y))
		}

		game.end.pos_x = scene.end.pos_x;
		game.end.pos_y = scene.end.pos_y;

		for (var i = 0; i < 100; i++) {
			var random_x = Math.random() * game.width;
			var random_y = Math.random() * game.height;
			game.stars.push(new game.make.star(random_x, random_y, 1));
		}
	},

	update_g_factor_high: function(e) {
		game.g_factor = G_FACTOR_HIGH;
		game.clicked = true;
		e.preventDefault();
	},

	update_g_factor_low: function(e) {
		game.g_factor = G_FACTOR_LOW;
		game.clicked = false;
		e.preventDefault();
	},

	old_time: 0,

	loop: function(new_time) {
		

		var dt = (new_time - game.old_time) / 1000;
		if (isNaN(dt)) dt = 0;
		game.old_time = new_time;

		game.update(dt);
		game.render();

		requestAnimFrame(game.loop);
	},

	update: function(dt) {
		if (Math.abs(game.player.pos_x - game.end.pos_x) < 7 && Math.abs(game.player.pos_y - game.end.pos_y) < 7) {
			game.is_finished = true;
			setTimeout(function() {
				location.reload();
			}, 2000);
		}

		game.player.update(dt);
		game.end.update(dt);
		for (var i = 0; i < game.black_holes.length; i++) {
			game.black_holes[i].update(dt);
		}
		for (var i = 0; i < game.stars.length; i++) {
			game.stars[i].update(dt);
		}
		for (var i = 0; i < game.particles.length; i++) {
			game.particles[i].update(dt);
			if (game.particles[i].done == true) game.particles.splice(i, 1);
		}
		game.camera.update();
	},

	render: function() {
		this.draw.clear();
		
		for (var i = 0; i < game.stars.length; i++) {
			game.stars[i].render(game.camera);
		}

		if (game.is_finished == true) {
			game.draw.circle(60, 50, 20, "blue");
			game.draw.circle(70, 58, 4, "green");
			game.draw.circle(50, 48, 8, "green");
			game.draw.circle(52, 58, 5, "green");
			game.draw.circle(68, 43, 6, "green");
			game.draw.text("You Win!", 65, 80, 24, "white");
		}
		else {
			for (var i = 0; i < game.black_holes.length; i++) {
				game.black_holes[i].render(game.camera);
			}
			game.end.render(game.camera);
			for (var i = 0; i < game.particles.length; i++) {
				game.particles[i].render(game.camera);
			}
			game.player.render(game.camera);
		}
	}
};

game.make = {
	black_hole: function(x, y) {
		this.pos_x = x;
		this.pos_y = y;
		this.size = BLACK_HOLE_SIZE;
		this.color = null;

		this.force_x = function(player_x, player_y) {
			var dx = this.pos_x - player_x;
			var dy = this.pos_y - player_y;
			var r2 = Math.pow(dx, 2) + Math.pow(dy, 2);
			var force_total = game.g_factor / Math.sqrt(r2);
			var theta = Math.atan2(dy, dx);
			return force_total * Math.cos(theta)
		}

		this.force_y = function(player_x, player_y) {
			var dx = this.pos_x - player_x;
			var dy = this.pos_y - player_y;
			var r2 = Math.pow(dx, 2) + Math.pow(dy, 2);
			var force_total = game.g_factor / Math.sqrt(r2);
			var theta = Math.atan2(dy, dx);
			return force_total * Math.sin(theta)
		}

		this.counter = 0;

		this.update = function(dt) {
			if (this.counter % 20 == 0) game.particles.push(new game.make.black_hole_particle(this.pos_x, this.pos_y, this.size, game.clicked));
			this.counter++;
		}

		this.render = function(camera) {
			this.color = game.ctx.createRadialGradient(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.pos_x - camera.x, this.pos_y - camera.y, this.size-1);
			this.color.addColorStop(0,"rgba(237,247,141,0)");
			this.color.addColorStop(1,"black");
			game.draw.circle(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.color);
		}
	},

	star: function(x, y) {
		this.pos_x = x;
		this.pos_y = y;
		this.color = "white";

		this.counter = ~~(Math.random() * 55 + 200); //number between 200 & 255 (inclusive)
		this.step = ~~(Math.random() * 4 + 4);
		this.up = true

		this.update = function() {
			if (this.up == true) this.counter += this.step;
			else this.counter -= this.step;
			if (this.counter > 255) {
				this.counter = 255;
				this.up = false;
			}
			if (this.counter < 200) {
				this.counter = 200;
				this.up = true;
			}
			this.color = "rgb("+this.counter+","+this.counter+","+this.counter+")";
		}

		this.render = function(camera) {
			game.draw.circle(this.pos_x - camera.x / 50, this.pos_y - camera.y / 50, 1, this.color)
		}
	},

	player_particle: function(x, y) {
		this.pos_x = x;
		this.pos_y = y;
		this.size = 2;
		this.opacity = 127;
		this.color = null;

		this.done = false;

		this.update = function() {
			this.opacity -= 1;
			this.size = this.opacity / 127;
			this.color = "rgba(128,128,128,"+this.opacity+")";
			if (this.size <= 0) this.done = true;
		}

		this.render = function(camera) {
			game.draw.circle(this.pos_x - camera.x, this.pos_y - camera.y, this.size, this.color)
		}
	},

	black_hole_particle: function(x, y, r, fast) {
		var theta = Math.random() * 2 * Math.PI;
		var vel = (fast) ? 24 : 8;

		this.pos_x = Math.cos(theta) * (r + 4) + x;
		this.pos_y = Math.sin(theta) * (r + 4) + y;
		this.vel_x = - Math.cos(theta) * vel;
		this.vel_y = - Math.sin(theta) * vel; 
		this.color = null;

		this.life = 128;
		this.reduction = (fast) ? 3 : 1;
		this.done = false;

		this.update = function(dt) {
			this.pos_x += this.vel_x * dt;
			this.pos_y += this.vel_y * dt;
			this.life -= this.reduction;
			if (this.life < 0) this.done = true;
			this.color = "rgb("+this.life+","+this.life+","+this.life+")";
			this.size = this.life/128;
		}

		this.render = function(camera) {
			game.draw.circle(this.pos_x - camera.x, this.pos_y - camera.y, 1, this.color);
		}
	}
}

var scene1 = {
	player: {
		pos_x: 0,
		pos_y: 90,
		vel_x: 100,
		vel_y: 0
	},
	black_holes: [
		{ pos_x: 150, pos_y: 70 },
		{ pos_x: 50, pos_y: 50}
	],
	end: {
		pos_x: 150,
		pos_y: 120
	}
}

window.requestAnimFrame = (function(){
  	return  window.requestAnimationFrame       || 
            window.webkitRequestAnimationFrame || 
            window.mozRequestAnimationFrame    || 
            window.oRequestAnimationFrame      || 
          	window.msRequestAnimationFrame     || 
          	function(callback){
            	window.setTimeout(callback, 1000 / 60);
          	};
})();
window.addEventListener("load", game.init, false);
window.addEventListener("resize", game.resize, false);

window.addEventListener("mousedown", game.update_g_factor_high, false);
window.addEventListener("mouseup", game.update_g_factor_low, false);
window.addEventListener("touchstart", game.update_g_factor_high, false);
window.addEventListener("touchend", game.update_g_factor_low, false);

window.addEventListener("keydown", function(e) {e.preventDefault();}, false);

game.init();
game.loop();