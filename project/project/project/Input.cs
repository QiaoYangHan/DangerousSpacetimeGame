using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace project
{
    class Input
    {
        private KeyboardState keyboardState;
        private KeyboardState lastState;
        private MouseState mouseState;
        private MouseState lastMouseState;

        public Input()
        {
            keyboardState = Keyboard.GetState();
            lastState = keyboardState;
            mouseState = Mouse.GetState();
            lastMouseState = mouseState;
        }

        public void Update()
        {
            lastState = keyboardState;
            keyboardState = Keyboard.GetState();
            lastMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public bool Right
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.Right) && lastState.IsKeyUp(Keys.Right);
                }
                else if (Game1.gamestate == Game1.GameStates.RP1CP2)
                {
                    return keyboardState.IsKeyDown(Keys.D) && lastState.IsKeyUp(Keys.D);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Right)
                        || keyboardState.IsKeyDown(Keys.D);
                }
            }
        }

        public bool LeftMouse
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return false;
                }
                else
                    return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
            }
        }

        public bool Left
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.Left) && lastState.IsKeyUp(Keys.Left);
                }
                else if (Game1.gamestate == Game1.GameStates.RP1CP2)
                {
                    return keyboardState.IsKeyDown(Keys.A) && lastState.IsKeyUp(Keys.A);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Left)
                        || keyboardState.IsKeyDown(Keys.A);
                }
            }
        }

        public bool Up
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return (keyboardState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up))
                        || (keyboardState.IsKeyDown(Keys.W) && lastState.IsKeyUp(Keys.W));
                }
                else if (Game1.gamestate == Game1.GameStates.RunningPlayer)
                {
                    return (keyboardState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up)) 
                        || (keyboardState.IsKeyDown(Keys.W) && lastState.IsKeyUp(Keys.W))
                        || (keyboardState.IsKeyDown(Keys.Space) && lastState.IsKeyUp(Keys.Space));
                }
                else if (Game1.gamestate == Game1.GameStates.RP1CP2)
                {
                    return keyboardState.IsKeyDown(Keys.W) && lastState.IsKeyUp(Keys.W);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Up)
                        || keyboardState.IsKeyDown(Keys.W);
                }
            }
        }

        public bool Down
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return (keyboardState.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down))
                        || (keyboardState.IsKeyDown(Keys.S) && lastState.IsKeyUp(Keys.S));
                }
                else if (Game1.gamestate == Game1.GameStates.RP1CP2)
                {
                    return keyboardState.IsKeyDown(Keys.S) && lastState.IsKeyUp(Keys.S);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Down)
                        || keyboardState.IsKeyDown(Keys.S);
                }
            }
        }

        public bool Tab
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.RP1CP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.RP1TP2)
                {
                    return keyboardState.IsKeyDown(Keys.Tab) && lastState.IsKeyUp(Keys.Tab);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Comma
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.C) && lastState.IsKeyUp(Keys.C);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Q
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.Q) && lastState.IsKeyUp(Keys.Q);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool E
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.E) && lastState.IsKeyUp(Keys.E);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool X
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.X) && lastState.IsKeyUp(Keys.X);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool C
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.C) && lastState.IsKeyUp(Keys.C);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Eheld
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.E);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Qheld
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.CP1RP2 || Game1.gamestate == Game1.GameStates.RunningPlayer
                    || Game1.gamestate == Game1.GameStates.TP1RP2)
                {
                    return keyboardState.IsKeyDown(Keys.Q);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool MenuSelect
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter);
            }
        }
    }
}
