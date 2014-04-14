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
        private Texture2D playerSprite;

        public Vector2 getPosition()
        {
            return position; 
        }private Vector2 position;

        public Vector2 getVelocity()
        {
            return velocity;
        }private Vector2 velocity;

        private Vector2 origin;
        private Vector2 previousPosition;

        bool isOnGround;
        bool isCollide;

        private Color[] spriteData;

        // horizontal motion, the object accelerates to its top speed
        private const float GroundAcceleration = 13000.0f;
        private const float MaxSlideSpeed = 3200.0f;
        // friction is taken into account whenever the object is moving on the ground or in the air
        private const float GroundFriction = 0.48f;
        private const float AirFriction = 0.58f;

        // vertical motion, the object falls to its top speed
        private const float GravityAcceleration = 2200.0f;
        private const float MaxFallSpeed = 450.0f;
        //factors in physics law specially designed for this game  
        private const float LaunchVelocity = -3000.0f;
        private const float JumpControlPower = 0.14f;
        private const float MaxJumpTime = 0.30f;

        //attribute that describs player's motion
        private float motion;
        private bool wasJumping;
        private bool isJumping;
        private float jumpTime;

        //collision detection
        public Rectangle playerRectangle;

        public Player(Vector2 position, ContentManager content)
        {
            playerSprite = content.Load<Texture2D>(@"Sprites/player");
            Reset(position);
        }

        public void Reset(Vector2 pos)
        {
            position = pos;
            velocity = Vector2.Zero;
            playerRectangle = new Rectangle(0, 0, playerSprite.Width, playerSprite.Height);
            origin = new Vector2(playerSprite.Width, playerSprite.Height) * 0.5f;
            isCollide = false;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            GetInput(keyboardState);
            physicsLaw(gameTime);

            // after player finishes his action, stop his motion
            motion = 0.0f;
        }

        private void GetInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left)
                || keyboardState.IsKeyDown(Keys.A))
                motion = -2.0f;

            else if (keyboardState.IsKeyDown(Keys.Right)
                     || keyboardState.IsKeyDown(Keys.D))
                motion = 2.0f;

            if (keyboardState.IsKeyDown(Keys.Space)
                || keyboardState.IsKeyDown(Keys.Up)
                || keyboardState.IsKeyDown(Keys.W))
                isJumping = true;
                //position.Y -= 2f;

            else if (keyboardState.IsKeyDown(Keys.Down)
                || keyboardState.IsKeyDown(Keys.S))
                //isJumping = true;
                //position.Y += 2f;
        }

        public void MoveLeft()
        {
            motion = -2.0f;
        }

        public void MoveRight()
        {
            motion = 2.0f;
        }

        public void MoveUp()
        {
            isJumping = true;
        }

        // player moves according to the physical law
        public void physicsLaw(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            previousPosition = position;

            //handle horizontal velocity
            velocity.X += motion * GroundAcceleration * elapsed;

            //vertical velocity isn't allowed to extend max fall speed
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);
            velocity.Y = Jump(velocity.Y, gameTime);

            // horizontal friction
            if (isOnGround)
                velocity.X *= GroundFriction;
            // vertical friction
            else
                velocity.X *= AirFriction;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxSlideSpeed, MaxSlideSpeed);
            //finalize the position with current velocity

            position += velocity * elapsed;
            position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));
            
            // separate player and tiles if the player is colliding with them 
            CollisionHandler();

            if (position.X == previousPosition.X)
            {
                velocity.X = 0;
            }
            if (position.Y == previousPosition.Y)
            {
                velocity.Y = 0;
            }
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
            List<Sprite> spriteList = new List<Sprite>();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    spriteList.Add(ShipKernel.layout[i, j]);
                }
            }
            Matrix playerTransform = Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                // Matrix.CreateScale(block.Scale) *  would go here
                                        Matrix.CreateRotationZ(0) *
                                        Matrix.CreateTranslation(new Vector3(position, 0.0f));

            Rectangle playerRect = Collision.CalculateBoundingRectangle(playerRectangle, playerTransform);

            for (int i = 0; i < spriteList.Count; i++)
            {
                // Build the block's transform
                if (spriteList[i] != null)
                {
                    Matrix spriteTransform =
                        Matrix.CreateTranslation(new Vector3(-spriteList[i].getOrigin(), 0.0f)) *
                        // Matrix.CreateScale(block.Scale) *  would go here
                        Matrix.CreateRotationZ(0) *
                        Matrix.CreateTranslation(new Vector3(spriteList[i].getPosition(), 0.0f));

                    // Calculate the bounding rectangle of this block in world space
                    Rectangle spriteRectangle = Collision.CalculateBoundingRectangle(spriteList[i].getSpriteRec(), spriteTransform);

                    if (playerRect.Intersects(spriteRectangle))
                    {
                        //Check collision with ship
                        if (Collision.IntersectPixels(playerTransform, (int)playerSprite.Width,
                                                     (int)playerSprite.Height, spriteData,
                                                     spriteTransform, (int)spriteList[i].getWidth(),
                                                     (int)spriteList[i].getHeight(), spriteList[i].getSpriteData()))
                        {
                            isOnGround = true;
                            position = new Vector2(position.X, position.Y - 4);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerSprite, position, Color.White);    
        }

        public void colorHandler()
        {
            spriteData = new Color[(int)playerSprite.Width * (int)playerSprite.Height];
            playerSprite.GetData(spriteData);
        }
    }
    
}
