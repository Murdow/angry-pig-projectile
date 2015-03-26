using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LancamentoProjetil
{
    /// <summary>
    /// Simulaçao Fisica.... Lançamento De Projetil....
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        Texture2D pig;

        Texture2D initMessage;
        Texture2D endMessage;
        Texture2D scoreMessage;
        bool init;
        bool end;        

        float positionX;
        float positionY;
        int velocidadeInicial;
        int ang;
        int gravidade;
        float rad;
        double pi;
        float time;
        float vox;
        float voy;
        float y;
        float x;
        bool fire;
        bool setAngulo;
        bool setForca;
        //Imprime rastro dos vetores com as posiçoes
        List<Positions> positions;
        Texture2D vector;
        SpriteFont position;
        bool drawPts;
        bool setPts;
        
        //MouseState mouseStateCurrent;
       //MouseState mouseStatePrevious;
        MouseState cursorPosition;
        Texture2D cursor;
        Texture2D cursorSelect;
                
        SpriteFont angulo;
        SpriteFont forca;
        SpriteFont commands;

        KeyboardState newState;
        KeyboardState oldState;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;//Seta as dimensoes da janela
            graphics.PreferredBackBufferHeight = 768;
        }

        protected override void Initialize()
        {
            positionX = 0;
            positionY = 768 - 69;
            gravidade = 10;
            time = 0;
            pi = 3.1415;
            ang = 0;
            velocidadeInicial = 0;
            y = 0;
            x = 0;            
            fire = false;
            setAngulo = true;
            setForca = false;
            positions = new List<Positions>();
            init = true;
            end = false;

            drawPts = false;
            setPts = false;
                       
            //Deixa o mouse visivel
            //this.IsMouseVisible = true;

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("background");
            pig = Content.Load<Texture2D>("pig");
            angulo = Content.Load<SpriteFont>("font");
            forca = Content.Load<SpriteFont>("font");
            commands = Content.Load<SpriteFont>("font");

            vector = Content.Load<Texture2D>("vector");
            position = Content.Load<SpriteFont>("positions");
            initMessage = Content.Load<Texture2D>("initMessage");
            endMessage = Content.Load<Texture2D>("again");
            scoreMessage = Content.Load<Texture2D>("score");

            cursor = Content.Load<Texture2D>("cursor");
            cursorSelect = Content.Load<Texture2D>("cursorSelect");
        }

      
        protected override void UnloadContent()
        {           
        }

      
        protected override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();
            //pega a posição do cursor....
            cursorPosition = Mouse.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                rad = ang * (float)pi / 180;
                vox = velocidadeInicial * (float)Math.Cos(rad);
                voy = velocidadeInicial * (float)Math.Sin(rad);
                fire = true;
                init = false;
            }
            /*if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
            {
                //backgroundChange = true;                
            }*/

            if (newState.IsKeyUp(Keys.Right) && oldState.IsKeyDown(Keys.Right))
            {
                if (setAngulo == true)
                    ang += 10;
                else if (setForca == true)
                    velocidadeInicial += 10;
                else if (setPts == true)
                    drawPts = true;

                if (ang > 90)
                    ang = 90;
                if (velocidadeInicial > 100)
                    velocidadeInicial = 100;
            }

            if (newState.IsKeyUp(Keys.Left) && oldState.IsKeyDown(Keys.Left))
            {
                if (setAngulo == true)
                    ang -= 10;
                else if (setForca == true)
                    velocidadeInicial -= 10;
                else if (setPts == true)
                    drawPts = false;

                if (ang < 0)
                    ang = 0;
                if (velocidadeInicial < 0)
                    velocidadeInicial = 0;
            }

            if (newState.IsKeyUp(Keys.Up) && oldState.IsKeyDown(Keys.Up))
            {
                if (setForca == true)
                {
                    setForca = false;
                    setAngulo = true;
                }
                else if (setPts == true)
                {
                    setPts = false;
                    setForca = true;
                }
            }
            if (newState.IsKeyUp(Keys.Down) && oldState.IsKeyDown(Keys.Down))
            {
                if (setAngulo == true)
                {
                    setAngulo = false;
                    setForca = true;
                }
                else if(setForca == true)
                {
                    setForca = false;
                    setPts = true;
                }
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Initialize();
            }
            //Simulaçao....
            if (fire == true)
            {                    
                    time += 0.2f;
                    x = vox * time;
                    y = voy * time - 0.5f * gravidade * (time * time);
                    positionX = x;
                    positionY = 699 - y;

                    Positions pst = new Positions(vector, new Vector2(positionX, positionY));
                    positions.Add(pst);

                    if (positionY >= 699)
                    {
                        fire = false;
                        end = true;                        
                    }
            }           

            positionX = MathHelper.Clamp(positionX, 0, 1024 - 60);
            positionY = MathHelper.Clamp(positionY, -100, 768 - 69);

            base.Update(gameTime);
            oldState = newState;
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
           
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            //Imprime baloes de texto
            if (init == true)
            {
                spriteBatch.Draw(initMessage, new Rectangle((int)positionX + 50, (int)positionY - 100, 120, 140), Color.White);
            }
            if (end == true)
            {
                spriteBatch.Draw(endMessage, new Rectangle((int)positionX - 150, (int)positionY - 120, 120, 140), Color.White);
                spriteBatch.Draw(scoreMessage, Vector2.Zero, Color.White);
            }

            //Desenha o rastro
            if (drawPts == true)
            {
                for (int i = 0; i < positions.Count; i++)
                {
                    spriteBatch.Draw(positions[i].texture, new Rectangle((int)positions[i].position.X, (int)positions[i].position.Y, 40, 38), Color.White);
                    spriteBatch.DrawString(position, "X: " + (int)positions[i].position.X, new Vector2(positions[i].position.X + 42, positions[i].position.Y), Color.Black);
                    spriteBatch.DrawString(position, "Y: " + (int)positions[i].position.Y, new Vector2(positions[i].position.X + 100, positions[i].position.Y), Color.Black);
                }
            }

            spriteBatch.Draw(pig, new Rectangle((int)positionX, (int)positionY, 60, 69), Color.White);

            if (setAngulo == true)
            {
                spriteBatch.DrawString(angulo, "Angulo: " + ang, new Vector2(0, 10), Color.Red);
            }
            else if (setAngulo == false)
            {
                spriteBatch.DrawString(angulo, "Angulo: " + ang, new Vector2(0, 10), Color.White);
            }
            if (setForca == true)
            {
                spriteBatch.DrawString(forca, "Forca: " + velocidadeInicial, new Vector2(0, 40), Color.Red);
            }
            else if (setForca == false)
            {
                spriteBatch.DrawString(forca, "Forca: " + velocidadeInicial, new Vector2(0, 40), Color.White);
            }
            if (setPts == true)
            {
                spriteBatch.DrawString(forca, "Rastro: " + drawPts, new Vector2(0, 70), Color.Red);
            }
            else if (setPts == false)
            {
                spriteBatch.DrawString(forca, "Rastro: " + drawPts, new Vector2(0, 70), Color.White);
            }
            spriteBatch.DrawString(commands, "Opcao selecionada: Vermelho", new Vector2(480, 10), Color.White);
            spriteBatch.DrawString(commands, "Up Arrow / Down Arrow: Seleciona", new Vector2(480, 40), Color.White);
            spriteBatch.DrawString(commands, "Numeros: Escolhe angulo e forca", new Vector2(480, 70), Color.White);            
            spriteBatch.DrawString(commands, "Enter: Dispara", new Vector2(480, 100), Color.White);
            spriteBatch.DrawString(commands, "Restart: R", new Vector2(480, 130), Color.White);
            spriteBatch.DrawString(commands, "X: " + (int)positionX + " Y: " + (int)positionY, new Vector2(480, 160), Color.White);

            spriteBatch.Draw(cursor, new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, 30, 30), Color.White);
            if (cursorPosition.X >= positionX && cursorPosition.X <= positionX + 60 && cursorPosition.Y >= positionY && cursorPosition.Y < positionY + 69)
            {
                spriteBatch.Draw(cursorSelect, new Rectangle((int)cursorPosition.X, (int)cursorPosition.Y, 30, 30), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
