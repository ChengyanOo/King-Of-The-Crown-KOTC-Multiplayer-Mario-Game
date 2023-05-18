namespace Sprint1.Factories.SpriteFactories
{
    public enum SpriteEnum
    {
        //Player
        player = 0x1000,
        player1 = 0x1,
        player2 = 0x0,

        dead = 0x0100,
        small = 0x0200,
        super = 0x0400,
        fire = 0x0800,

        idle = 0x0002,
        running = 0x0004,
        jumping = 0x0008,
        falling = 0x0010,
        crouching = 0x0020,

        allPowers = player | player1 | player2 | dead | small | super | fire,
        allActions = player | player1 | player2 | idle | running | jumping | falling | crouching,
        allPlayers = allPowers | allActions,

        //Crown
        crown = 0x2000,
        floating = 0x0100,
        thrown = 0x0400,
        attached = 0x0800,

        allCrowns = crown | floating | thrown | attached,

        //Enemy
        enemy = 0x4000,

        koopa = 0x0010,
        goomba = 0x0020,
        deadGoomba = 0x0040,
        shellKoopa = 0x0080,
        piranhaPlant = 0x0100,

        allEnemies = enemy | koopa | goomba | deadGoomba | shellKoopa | piranhaPlant,

        //Item
        item = 0x8000,

        smallCoin = 0x0010,
        bigCoin = 0x0020,
        greenMushroom = 0x0040,
        redMushroom = 0x0080,
        fireFlower = 0x0100,
        star = 0x0200,

        allItems = item | smallCoin | bigCoin | greenMushroom | redMushroom | fireFlower | star,

        //Positions
        top = 0x0002,
        bot = 0x0004,
        left = 0x0008,
        right = 0x0010,
        mid = 0x0020,

        //Block
        block = 0x10000,

        platform = 0x0100,
        tapered = 0x0040,
        moving = 0x0080,

        wall = 0x0200,

        brick = 0x0010,
        hidden = 0x0020,
        floor = 0x0100,
        stair = 0x0200,
        jumpPad = 0x0400,

        spawning = 0x0001,

        allPlatforms = block | platform | top | bot | left | right | mid | tapered,
        allWalls = block | wall | top | bot | left | right | mid,
        allFloors = block | floor | top | bot | left | right,
        allBlocks = block | brick | hidden | stair | allPlatforms | allWalls | allFloors,

        //Particle
        particle = 0x20000,
        //Fireball
        fireball = 0x0020,
        explosion = 0x0040,

        allParticles = particle | brick | fireball | explosion,

        //Hazard
        hazard = 0x40000,
        spike = 0x0100,
        muncher = 0x0004,
        hothead = 0x0008,

        allHazards = block | hazard | spike | muncher | hothead,
    }
}