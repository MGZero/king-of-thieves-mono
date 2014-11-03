﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace King_of_Thieves.Actors.HUD.health
{
    class CHealth : CHUDElement
    {
        private int _portionFilled = 0;
        private int _heartNumber = 0;
        private bool _isActive = false;
        private static Graphics.CSprite[] _imageCache = null;
        private const int _MAX_HEARTS_PER_ROW = 10;
        private Vector2 _fixedPosition = Vector2.Zero;

        public CHealth(int heartNumber, int portionFilled, bool isActive)
        {
            _initImageCache();

            _heartNumber = heartNumber;
            _portionFilled = portionFilled;
            _isActive = isActive;

            _position = new Microsoft.Xna.Framework.Vector2();

            _position.Y = _heartNumber <= _MAX_HEARTS_PER_ROW ? 9 : 16;

            if (heartNumber <= 10)
                _position.X = heartNumber * _MAX_HEARTS_PER_ROW;
            else
                _position.X = (heartNumber - _MAX_HEARTS_PER_ROW) * 10;

            _fixedPosition = _position;

            if (_isActive && _portionFilled == 4)
                image = _imageCache[0];
            else if (_portionFilled == 4)
                image = _imageCache[1];
            else
                image = _imageCache[5 - _portionFilled];
        }

        private void _initImageCache()
        {
            //only do this ONCE
            if (_imageCache == null)
            {
                _imageCache = new Graphics.CSprite[6];

                for (int i = 0; i < 6; i++)
                    _imageCache[i] = new Graphics.CSprite("HUD:health" + i);
            }
        }

        public int portionFilled
        {
            set
            {
                _portionFilled = value;
                image = _imageCache[5 - _portionFilled];
            }
        }

        public void flipActive(bool setToZero = false)
        {
            _isActive = !_isActive;

            if (setToZero)
                portionFilled = 0;
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
