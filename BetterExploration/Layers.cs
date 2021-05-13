using System;

namespace Slothsoft.BetterExploration {
    [Flags]
    enum Layers {
        Default = 1 << 0,
        TransparentFX = 1 << 1,
        IgnoreRaycast = 1 << 2,
        Unused3 = 1 << 3,
        Water = 1 << 4,
        UI = 1 << 5,
        Unused6 = 1 << 6,
        Unused7 = 1 << 7,
        effect = 1 << 8,
        character = 1 << 9,
        piece = 1 << 10,
        terrain = 1 << 11,
        item = 1 << 12,
        ghost = 1 << 13,
        character_trigger = 1 << 14,
        static_solid = 1 << 15,
        piece_nonsolid = 1 << 16,
        character_ghost = 1 << 17,
        hitbox = 1 << 18,
        skybox = 1 << 19,
        Default_small = 1 << 20,
        WaterVolume = 1 << 21,
        weapon = 1 << 22,
        blocker = 1 << 23,
        pathblocker = 1 << 24,
        viewblock = 1 << 25,
        character_net = 1 << 26,
        character_noenv = 1 << 27,
        vehicle = 1 << 28,
        Unused29 = 1 << 29,
        Unused30 = 1 << 30,
        smoke = 1 << 31,
    }
}
