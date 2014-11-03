using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace King_of_Thieves.Actors.HUD.buttons
{
    class CButton : CHUDElement
    {
        public enum HUD_BUTTON_TYPE
        {
            LEFT = 0,
            RIGHT
        }

        private readonly HUD_BUTTON_TYPE _BUTTON_TYPE = 0;

        private static string _BUTTON_LEFT = "buttonLeft";
        private static string _BUTTON_RIGHT = "buttonRight";
        public CButton(HUD_BUTTON_TYPE type)
        {
            _BUTTON_TYPE = type;

            switch (_BUTTON_TYPE)
            {
                case HUD_BUTTON_TYPE.LEFT:
                    _imageIndex.Add(_BUTTON_LEFT, new Graphics.CSprite("HUD:buttonLeft"));
                    swapImage(_BUTTON_LEFT);
                    _fixedPosition = new Vector2(250, 10);
                    break;

                case HUD_BUTTON_TYPE.RIGHT:
                    _imageIndex.Add(_BUTTON_RIGHT, new Graphics.CSprite("HUD:buttonRight"));
                    swapImage(_BUTTON_RIGHT);
                    _fixedPosition = new Vector2(283, 10);
                    break;
            }
        }

        public override void drawMe(bool useOverlay = false)
        {
            base.drawMe(useOverlay);
        }

        public override void update(GameTime gameTime)
        {
            _position.X = _fixedPosition.X - CMasterControl.camera.position.X;
            _position.Y = _fixedPosition.Y - CMasterControl.camera.position.Y;
        }
    }
}
