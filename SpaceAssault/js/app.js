var requestAnimFrame = (function(){
    return window.requestAnimationFrame       ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame    ||
        window.oRequestAnimationFrame      ||
        window.msRequestAnimationFrame     ||
        function(callback){
            window.setTimeout(callback, 1000 / 60);
        };
})();

var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 512;
canvas.height = 480;
document.body.appendChild(canvas);

var lastTime;
function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimFrame(main);
};

function init() {
    terrainPattern = ctx.createPattern(resources.get('img/terrain.png'), 'repeat');

    document.getElementById('play-again').addEventListener('click', function() {
        reset();
    });

    reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/sprites.png',
    'img/terrain.png'
]);
resources.onReady(init);

var player = {
    pos: [0, 0],
    sprite: new Sprite('img/sprites.png', [0, 6], [39, 24], 16, [0, 1])
};

var megaliths = [];
var mannas = [];
var bullets = [];
var enemies = [];
var explosions = [];

var lastFire = Date.now();
var gameTime = 0;
var isGameOver;
var terrainPattern;

var score = 0;
var scoreOfMannas = 0;
var scoreEl = document.getElementById('score');
var scoreOfMannasEl = document.getElementById('scoreOfMannas');

var playerSpeed = 200;
var bulletSpeed = 500;
var enemySpeed = 100;

function update(dt) {
    gameTime += dt;

    handleInput(dt);
    updateEntities(dt);

    if(Math.random() < 1 - Math.pow(.993, gameTime)) {
        enemies.push({
            pos: [canvas.width,
                  Math.random() * (canvas.height - 39)],
            sprite: new Sprite('img/sprites.png', [0, 78], [80, 39],
                               6, [0, 1, 2, 3, 2, 1])
        });
    }

    checkCollisions();

    scoreEl.innerHTML = score;
    scoreOfMannasEl.innerHTML = scoreOfMannas;
};

function handleInput(dt) {
    var currentPositionOfPlayer = player.pos.slice();
    
    if(input.isDown('DOWN') || input.isDown('s')) {
        player.pos[1] += playerSpeed * dt;
    }

    if(input.isDown('UP') || input.isDown('w')) {
        player.pos[1] -= playerSpeed * dt;
    }

    if(input.isDown('LEFT') || input.isDown('a')) {
        player.pos[0] -= playerSpeed * dt;
    }

    if(input.isDown('RIGHT') || input.isDown('d')) {
        player.pos[0] += playerSpeed * dt;
    }

    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;

        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            player.pos = currentPositionOfPlayer;
        }
    }

    if(input.isDown('SPACE') &&
       !isGameOver &&
       Date.now() - lastFire > 100) {
        var x = player.pos[0] + player.sprite.size[0] / 2;
        var y = player.pos[1] + player.sprite.size[1] / 2;

        bullets.push({ pos: [x, y],
                       dir: 'forward',
                       sprite: new Sprite('img/sprites.png', [0, 39], [18, 8]) });
        bullets.push({ pos: [x, y],
                       dir: 'up',
                       sprite: new Sprite('img/sprites.png', [0, 50], [9, 5]) });
        bullets.push({ pos: [x, y],
                       dir: 'down',
                       sprite: new Sprite('img/sprites.png', [0, 60], [9, 5]) });

        lastFire = Date.now();
    }
}

function updateEntities(dt) {
    player.sprite.update(dt);

    for(var i=0; i<megaliths.length; i++) {
        megaliths[i].sprite.update(dt);
    }

    for(var i=0; i<mannas.length; i++) {
        mannas[i].sprite.update(dt);
    }

    for(var i=0; i<bullets.length; i++) {
        var bullet = bullets[i];

        switch(bullet.dir) {
        case 'up': bullet.pos[1] -= bulletSpeed * dt; break;
        case 'down': bullet.pos[1] += bulletSpeed * dt; break;
        default:
            bullet.pos[0] += bulletSpeed * dt;
        }

        if(bullet.pos[1] < 0 || bullet.pos[1] > canvas.height ||
           bullet.pos[0] > canvas.width) {
            bullets.splice(i, 1);
            i--;
        }
    }

    for(var i=0; i<enemies.length; i++) {
        var enemy = enemies[i];

        switch(enemy.dir) {
        case 'up': enemy.pos[1] -= enemySpeed * dt; break;
        case 'down': enemy.pos[1] += enemySpeed * dt; break;
        default:
            enemy.pos[0] -= enemySpeed * dt;
        }
        
        enemies[i].sprite.update(dt);

        if(enemies[i].pos[0] + enemies[i].sprite.size[0] < 0) {
            enemies.splice(i, 1);
            i--;
        }
    }

    for(var i=0; i<explosions.length; i++) {
        explosions[i].sprite.update(dt);

        if(explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }
}

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
             b <= y2 || y > b2);
}

function boxCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
                    pos[0] + size[0], pos[1] + size[1],
                    pos2[0], pos2[1],
                    pos2[0] + size2[0], pos2[1] + size2[1]);
}

function checkCollisions() {
    checkPlayerBounds();
    
    for(var i=0; i<enemies.length; i++) {
        var pos = enemies[i].pos;
        var size = enemies[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                enemies.splice(i, 1);
                i--;

                score += 100;

                explosions.push({
                    pos: pos,
                    sprite: new Sprite('img/sprites.png',
                                       [0, 117],
                                       [39, 39],
                                       16,
                                       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                                       null,
                                       true)
                });

                bullets.splice(j, 1);
            }
        }

        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            gameOver();
        }
    }

    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                bullets.splice(j, 1);
            }
        }
    }

    for(var i=0; i<enemies.length; i++) {
        var pos = enemies[i].pos;
        var size2 = enemies[i].sprite.size;

        for(var j=0; j<megaliths.length; j++) {
            var pos2 = megaliths[j].pos;
            var size2 = megaliths[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                if (enemies[i].dir === 'down' || pos2[1] < pos[1] - size[1]/3) {
                    enemies[i].dir = 'down';
                }
                else {
                    enemies[i].dir = 'up';
                }
                break;
            }
            else {
                enemies[i].dir = 'horizontal';
            }
        }
    }

    for(var i=0; i<mannas.length; i++) {
        var pos = mannas[i].pos;
        var size = mannas[i].sprite.size;

        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            mannas.splice(i, 1);
            i--;

            scoreOfMannas += 1;

            explosions.push({
                pos: pos,
                sprite: new Sprite('img/sprites.png',
                                   [12, 162],
                                   [52, 41],
                                   16,
                                   [0, 1, 2, 3],
                                   null,
                                   true)
            });

            var countOfNewMannas = Math.floor(Math.random() * (9 - mannas.length));

            for (j=0; j<countOfNewMannas; j++) {
                mannas.push({
                    pos: getPositionOfMannas(),
                    sprite: new Sprite('img/sprites.png', [12, 162], [52, 41], 4, [0, 1])
                });
            }
        }
    }
}

function checkPlayerBounds() {
    if(player.pos[0] < 0) {
        player.pos[0] = 0;
    }
    else if(player.pos[0] > canvas.width - player.sprite.size[0]) {
        player.pos[0] = canvas.width - player.sprite.size[0];
    }

    if(player.pos[1] < 0) {
        player.pos[1] = 0;
    }
    else if(player.pos[1] > canvas.height - player.sprite.size[1]) {
        player.pos[1] = canvas.height - player.sprite.size[1];
    }
}

function render() {
    ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    if(!isGameOver) {
        renderEntity(player);
    }

    renderEntities(megaliths);
    renderEntities(mannas);
    renderEntities(bullets);
    renderEntities(enemies);
    renderEntities(explosions);
};

function renderEntities(list) {
    for(var i=0; i<list.length; i++) {
        renderEntity(list[i]);
    }    
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
}

function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;
    scoreOfMannas = 0;
    megaliths = [];
    mannas = [];
    enemies = [];
    bullets = [];
    player.pos = [50, canvas.height / 2];
    
    countOfMegaliths = Math.floor(3 + Math.random() * 3);
    for(var i=0; i<countOfMegaliths; i++) {
        if (Math.random() < 0.5) {
            megaliths.push({
                pos: getPosition( [56, 54] ),
                sprite: new Sprite('img/sprites.png', [2, 212], [56, 54])
            });
        }
        else {
            megaliths.push({
                pos: getPosition( [49, 43] ),
                sprite: new Sprite('img/sprites.png', [4, 273], [49, 43])
            });
        }
    }

    countOfMannas = Math.floor(3 + Math.random() * 6);
    for(var i=0; i<countOfMannas; i++) {
        mannas.push({
            pos: getPositionOfMannas(),
            sprite: new Sprite('img/sprites.png', [12, 162], [52, 41], 4, [0, 1])
        });
    }
}

function getPositionOfMannas() {
    var position = getPosition([52, 41]);
    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;
        while (true) {
            if (!boxCollides(position, [52, 41], pos, size)) {
                break;
            }
            else {
                i = 0;
                position = getPosition([52, 41]);
            }
        }
    }
    return position;
}

function getPosition(size) {
    var position = getRandomPosition();
    while (true) {
        if (!boxCollides(position, size, player.pos, player.sprite.size)) {
            break;
        }
        else {
            position = getRandomPosition();
        }
    }
    return position;
}

function getRandomPosition() {
    return [40 + Math.random() * (canvas.width - 120), 40 + Math.random() * (canvas.height - 120)];
};