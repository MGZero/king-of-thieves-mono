using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace King_of_Thieves.Actors.Projectiles
{
    class CEnergyWave : CProjectile
    {
        public CEnergyWave(DIRECTION direction, Vector2 velocity, Vector2 position) :
            base(direction, velocity, position)
        {
            name = "energyWaveSmall";
            _imageIndex.Add(PROJ_DOWN, new Graphics.CSprite(Graphics.CTextures.EFFECT_ENERGY_WAVE_SMALL));
            _imageIndex.Add(PROJ_LEFT, new Graphics.CSprite(Graphics.CTextures.EFFECT_ENERGY_WAVE_SMALL));
            _imageIndex.Add(PROJ_RIGHT, new Graphics.CSprite(Graphics.CTextures.EFFECT_ENERGY_WAVE_SMALL));
            _imageIndex.Add(PROJ_UP, new Graphics.CSprite(Graphics.CTextures.EFFECT_ENERGY_WAVE_SMALL,false,true));

            _hitBox = new Collision.CHitBox(this, 6, 9, 14, 10);

            shoot();
            startTimer0(120);
        }

        public override void timer0(object sender)
        {
            base.timer0(sender);
            _killMe = true;
        }

        public override void drawMe(bool useOverlay = false)
        {
            base.drawMe(useOverlay);
        }
    }
}
