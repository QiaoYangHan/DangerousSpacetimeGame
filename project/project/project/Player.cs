using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace project
{
    class Player
    {
        enum Condition
        { 
            Space,
            Ship
        }

        private int playerNumber;

        public Texture2D texture;
        public Texture2D flag;
        private Texture2D walk;
        private Texture2D jump;
        private Color[] spriteData;
        public Vector2 oldPostion;

        public Rectangle currentFrame;

        public int frameWidth;
        public int frameHeight;
        int elapsed = 0;
        State currentState = State.Idle;
        Condition currentCondition = Condition.Ship;
        public bool drawFlag;

        enum State
        {
            Idle,
            Walk,
            Jump
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 LocalPosition
        {
            get { return localPosition; }
            set { localPosition = value; }
        }

        public void setSpace()
        {
            currentCondition = Condition.Space;
        }

        public bool isSpace()
        {
            return currentCondition == Condition.Space;
        }

        public void setShip()
        {
            currentCondition = Condition.Ship;
        }

        public Vector2 position;
        private Vector2 prevPosition;
        private Vector2 localPosition;
        private Vector2 playerSize;

        // horizontal motion, the object accelerates to its top speed
        private const float GroundAcceleration = 9000.0f;
        private const float MaxSlideSpeed = 2200.0f;
        // friction is taken into account whenever the object is moving on the ground or in the air
        private const float GroundFriction = 0.48f;
        private const float AirFriction = 0.58f;

        // vertical motion, the object falls to its top speed
        private const float GravityAcceleration = 2000.0f;
        private const float MaxFallSpeed = 300.0f;
        //factors in physics law specially designed for this game  
        private const float LaunchVelocity = -2000.0f;
        private const float JumpControlPower = 0.10f;
        private const float MaxJumpTime = 0.35f;

        //attribute that describs player's motion
        private float motion;
        private Vector2 velocity;
        private float jumpTime;

        private bool wasJumping;
        private bool isJumping;
        private bool isOnGround;

        //collision detection
        public Rectangle playerRectangle;
        private Rectangle localBounds;

        public Player(Vector2 position, ContentManager content, int pNum)
        {
            texture = content.Load<Texture2D>(@"Sprites/player");
            walk = content.Load<Texture2D>(@"Sprites/playerRun");
            jump = content.Load<Texture2D>(@"Sprites/playerJump");
            flag = content.Load<Texture2D>(@"Sprites/flag");
            Reset(position);
            localPosition.X = position.X - (float)((float)1900 / 2f - 85 * (float)Math.Sqrt(2));
            localPosition.Y = position.Y - (float)((float)950 / 2f - 85 * (float)Math.Sqrt(2));
            playerNumber = pNum;
            frameWidth = texture.Width;
            frameHeight = texture.Height;
            currentFrame = new Rectangle(0, 0, frameWidth, frameHeight);
            currentCondition = Condition.Ship;
            drawFlag = false;
        }

        public void Reset(Vector2 pos)
        {
            position = pos;
            oldPostion = position;
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (currentCondition == Condition.Ship)
            {
                GetInput(keyboardState);
                physicsLaw(gameTime);

                //if (isOnGround)
                //{
                //    if (Math.Abs(velocity.X) - 0.02f > 0)
                //    {
                //        sprite.PlayAnimation(runAnimation);
                //    }
                //    else
                //    {
                //        sprite.PlayAnimation(idleAnimation);
                //    }
                //}

                motion = 0.0f;
            }
            else if (currentCondition == Condition.Space)
            { 
                
            }
        }

        private void GetInput(KeyboardState keyboardState)
        {
            if (playerNumber == 2)
            {
                if (keyboardState.IsKeyDown(Keys.Left))
                    motion = -1.0f;

                if (keyboardState.IsKeyDown(Keys.Right))
                    motion = 1.0f;

                if (keyboardState.IsKeyDown(Keys.Up) && isOnGround == true)
                {
                    isJumping = true;
                    position.Y -= 2f;
                }
            }
            else if (playerNumber == 1)
            {
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    motion = -1.0f;
                    currentState = State.Walk;
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    motion = 1.0f;
                    currentState = State.Walk;
                }

                if (keyboardState.IsKeyDown(Keys.W) && isOnGround == true)
                {
                    isJumping = true;
                    position.Y -= 2f;
                    currentState = State.Jump;
                }
            }
        }

        // player moves according to the physical law
        public void physicsLaw(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            prevPosition = position;

            //handle horizontal velocity
            velocity.X += motion * GroundAcceleration * elapsed;

            // horizontal friction
            if (isOnGround)
                velocity.X *= GroundFriction;
            // vertical friction
            else
                velocity.X *= AirFriction;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxSlideSpeed, MaxSlideSpeed);

            //vertical velocity isn't allowed to extend max fall speed
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            velocity.Y = Jump(velocity.Y, gameTime);

            //finalize the position with current velocity
            position += velocity * elapsed;
            position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));
            
            // separate player and tiles if the player is colliding with them 
            CollisionHandler();

            if (position.X == prevPosition.X)
                velocity.X = 0;
            if (position.Y == prevPosition.Y)
                velocity.Y = 0;
        }

        private float Jump(float verticalVelocity, GameTime gameTime)
        {
            if(isJumping)
            {
                // begin to jump or in the jumping mode
                if((!wasJumping && isOnGround) || jumpTime > 0.0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // If player is jumping up
                if(0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // special physics law of free fall is applied here
                    verticalVelocity = LaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // reach the apex of the jump
                    isJumping = false;
                    jumpTime = 0.0f;
                }
            }

            else // player falls from the apex or slides on the ground or stays unmoved
            {
                jumpTime = 0.0f;
            }

            wasJumping = isJumping;
            return verticalVelocity;
        }

        //// collision detection
        private void CollisionHandler()
        {
            playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            // treat the width of a tile and the height of the tile as units
            int left = (int)MathHelper.Clamp((float)Math.Floor(((float)playerRectangle.Left - ShipKernel.layout[0, 0].getPosition().X ) / 6 ), 
                        0f, 39f);
            int right = (int)MathHelper.Clamp((float)Math.Ceiling(((float)playerRectangle.Right - ShipKernel.layout[0, 0].getPosition().X ) / 6) - 1, 
                        0f, 39f);
            int top = (int)MathHelper.Clamp((float)Math.Floor(((float)playerRectangle.Top - ShipKernel.layout[0, 0].getPosition().Y ) / 6) , 
                        0f, 39f);
            int bottom = (int)MathHelper.Clamp((float)Math.Ceiling(((float)playerRectangle.Bottom - ShipKernel.layout[0, 0].getPosition().Y) / 6) - 1, 
                        0f, 39f);

            // Reset flag to search for ground collision.
            isOnGround = false;

            // For each potentially colliding tile,
            for (int j = top; j <= bottom; ++j)
            {
                for (int i = left; i <= right; ++i)
                {
                    //Sprite tile = new Sprite(ShipKernel.layout[j, i].getSpriteImage(), ShipKernel.layout[j, i].getPosition());
                    
                    if (ShipKernel.layout[j, i] != null)
                    {
                        Vector2 depth = Tool.GetIntersectionDepth(playerRectangle, ShipKernel.layout[j, i].getSpriteRec());
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY < absDepthX)
                            {
                                if (previousBottom <= ShipKernel.layout[j, i].getSpriteRec().Top)
                                    isOnGround = true;

                                if (isOnGround)
                                {
                                    position = new Vector2(position.X, position.Y + depth.Y);
                                    playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                                }
                                if (j == 0)
                                {
                                    position = new Vector2(position.X, position.Y + absDepthY);
                                    playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                                }
                            }
                            else 
                            {
                                if (i == 0)
                                {
                                    position = new Vector2(position.X + absDepthX, position.Y);
                                    playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                                }
                                else if (i == 39)
                                {
                                    position = new Vector2(position.X - absDepthX, position.Y);
                                    playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                                }
                                position = new Vector2(position.X, position.Y);
                                playerRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
                            }
                        }
                    }
                }
            }
            previousBottom = playerRectangle.Bottom;
        }private float previousBottom;

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            // Draw that sprite.
            //spriteBatch.Draw(texture, Position, Color.White);
            if (elapsed > 15)
            {
                if (currentState == State.Idle)
                {
                    currentFrame.X = 0;
                    spriteBatch.Draw(texture, position, currentFrame, Color.White);
                }
                else if (currentState == State.Walk)
                {
                    if (currentFrame.X < 340)
                    {
                        spriteBatch.Draw(walk, position, currentFrame, Color.White);
                        currentFrame.X += 39;
                    }
                    else
                    {
                        currentFrame.X = 0;
                        spriteBatch.Draw(walk, position, currentFrame, Color.White);
                    }
                }
                else if (currentState == State.Jump)
                {
                    if (currentFrame.X < 340)
                    {
                        spriteBatch.Draw(jump, position, currentFrame, Color.White);
                        currentFrame.X += 39;
                    }
                    else
                    {
                        currentFrame.X = 0;
                        spriteBatch.Draw(jump, position, currentFrame, Color.White);
                    }
                }
                currentState = State.Idle;
                elapsed = 0;
            }
            // Flip the sprite to face the way we are moving.
            //if (velocity.X > 0)
            //    flip = SpriteEffects.FlipHorizontally;
            //else if (velocity.X < 0)
            //    flip = SpriteEffects.None;
            //sprite.Draw(gameTime, spriteBatch, position, flip);
        }

        public void colorHandler()
        {
            spriteData = new Color[(int)texture.Width * (int)texture.Height];
            texture.GetData(spriteData);
        }
    }
    
}
