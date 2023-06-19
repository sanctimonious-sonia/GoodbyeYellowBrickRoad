using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace GoodbyeYellowBrickRoad
{
    public partial class Form1 : Form
    {
        //create bricks
        Rectangle[] brownBricks = { new Rectangle(0, 250, 100, 30), new Rectangle(0, 285, 50, 30), new Rectangle(0,320,100,30), new Rectangle(0,355,50,30), new Rectangle(0,390,100,30), new Rectangle(525,250,100,30), new Rectangle(475,285,120,30), new Rectangle(525,320,100,30), new Rectangle(475,355,120,30), new Rectangle(525,390,100,30) };

        Rectangle[] greenBricks = {new Rectangle(105,250,100,30), new Rectangle(420,250,100,30), new Rectangle(55,285,100,30), new Rectangle(370,285,100,30), new Rectangle(105,320,100,30), new Rectangle(420,320,100,30), new Rectangle(55,355,100,30), new Rectangle(370,355,100,30), new Rectangle(105,390,100,30), new Rectangle(420,390,100,30) };

        Rectangle[] yellowBricks = {new Rectangle(210,250,100,30), new Rectangle(315,250,100,30), new Rectangle(160,285,100,30), new Rectangle(265,285,100,30), new Rectangle(210,320,100,30), new Rectangle(160,355,100,30), new Rectangle(265,355,100,30), new Rectangle(210,390,100,30), new Rectangle(315,390,100,30) };
       
        Rectangle bonusBrick = new Rectangle(315,320,100,30);
        Rectangle ball = new Rectangle(283,520,17,17);
        Rectangle platform = new Rectangle(200,540,200,15);
        Rectangle currentObject = new Rectangle();

        Stopwatch stopwatch = new Stopwatch();

        Random randGen = new Random();
        
        //global variables
        int score = 0;
        int lives = 5;
        int playerSpeed = 7;
        int ballXSpeed;
        int ballYSpeed;
        int missingBricks = 0;
        int bonusCompleted = 0;
        int bonusWon = 0;
        string difficulty;
        

        bool leftDown = false;
        bool rightDown = false;

        SolidBrush greyBrush = new SolidBrush(Color.FromArgb(198,171,142));
        SolidBrush brownBrush = new SolidBrush(Color.FromArgb(80,45,15));
        SolidBrush greenBrush = new SolidBrush(Color.FromArgb(72,109,34));
        SolidBrush yellowBrush = new SolidBrush(Color.FromArgb(251,210,52));
        SolidBrush purpleBrush = new SolidBrush(Color.FromArgb(134,81,78));
        SolidBrush blackBrush = new SolidBrush (Color.Black);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        Font font = new Font("Arial", 14, FontStyle.Regular);

        string status = "start";

       //start game
        public void InitializeGame()
        {
            introLabel.Text = "";
            easyButton.Enabled = false;
            easyButton.Visible = false;
            mediumButton.Enabled = false;
            mediumButton.Visible = false;
            hardButton.Enabled = false;
            hardButton.Visible = false;

            scoreLabel.Text = $"Score: {score}";
            livesLabel.Text = $"Lives: {lives}";

            gameTimer.Enabled = true;
            stopwatch.Start();
            status = "brick-breaking";
            
        }

        //reset bricks to original positions
        public void ResetBricks()
        {
            brownBricks[0] = new Rectangle(0, 250, 100, 30);
            brownBricks[1] = new Rectangle(0, 285, 50, 30);
            brownBricks[2] = new Rectangle(0, 320, 100, 30);
            brownBricks[3] = new Rectangle(0, 355, 50, 30);
            brownBricks[4] = new Rectangle(0, 390, 100, 30);
            brownBricks[5] = new Rectangle(525, 250, 100, 30);
            brownBricks[6] = new Rectangle(475, 285, 120, 30);
            brownBricks[7] = new Rectangle(525, 320, 100, 30);
            brownBricks[8] = new Rectangle(475, 355, 120, 30);
            brownBricks[9] = new Rectangle(525, 390, 100, 30);

            greenBricks[0] = new Rectangle(105, 250, 100, 30);
            greenBricks[1] = new Rectangle(420, 250, 100, 30);
            greenBricks[2] = new Rectangle(55, 285, 100, 30);
            greenBricks[3] = new Rectangle(370, 285, 100, 30);
            greenBricks[4] = new Rectangle(105, 320, 100, 30);
            greenBricks[5] = new Rectangle(420, 320, 100, 30);
            greenBricks[6] = new Rectangle(55, 355, 100, 30);
            greenBricks[7] = new Rectangle(370, 355, 100, 30);
            greenBricks[8] = new Rectangle(105, 390, 100, 30);
            greenBricks[9] = new Rectangle(420, 390, 100, 30);

            yellowBricks[0] = new Rectangle(210, 250, 100, 30);
            yellowBricks[1] = new Rectangle(315, 250, 100, 30);
            yellowBricks[2] = new Rectangle(160, 285, 100, 30);
            yellowBricks[3] = new Rectangle(265, 285, 100, 30);
            yellowBricks[4] = new Rectangle(210, 320, 100, 30);
            yellowBricks[5] = new Rectangle(160, 355, 100, 30);
            yellowBricks[6] = new Rectangle(265, 355, 100, 30);
            yellowBricks[7] = new Rectangle(210, 390, 100, 30);
            yellowBricks[8] = new Rectangle(315, 390, 100, 30);

            bonusBrick = new Rectangle(315, 320, 100, 30);

            missingBricks = 0;

            ball.X = 283;
            ball.Y = 520;

            if (difficulty == "easy")
            {
                ballXSpeed = 6;
                ballYSpeed = 6;
            }
            else if (difficulty == "medium")
            {
                ballXSpeed = 8;
                ballYSpeed = 8;
            }
            else
            {
                ballXSpeed = 10;
                ballYSpeed = 10;
            }

            Refresh();
        }

        //change ball direction upon hitting a brick
        public void IntersectionDirectionDetection()
        {
            Rectangle left = new Rectangle(ball.X, ball.Y+2, 2, ball.Height-4);
            Rectangle top = new Rectangle(ball.X+2, ball.Y, ball.Width-4, 2);
            Rectangle bottom = new Rectangle(ball.X+2, ball.Y + ball.Height-4, ball.Width-4, 2);
            Rectangle right = new Rectangle(ball.X + ball.Width-2, ball.Y+2, 2, ball.Height-4);

            if (currentObject.IntersectsWith(left) || currentObject.IntersectsWith(right))
            {
                ballXSpeed *= -1;
                
                if (ballXSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(5,8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(-11, -8);
                    }
                }
            }

            if (currentObject.IntersectsWith(top)|| currentObject.IntersectsWith(bottom))
            {
                ballYSpeed *= -1;

                if (ballYSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(5, 8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(-11, -8);
                    }
                }
            }

            if (bonusBrick.IntersectsWith(left) || bonusBrick.IntersectsWith(right))
            {
                ballXSpeed *= -1;

                if (ballXSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(5, 8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(-11, -8);
                    }
                }
            }

            if (bonusBrick.IntersectsWith(top) || bonusBrick.IntersectsWith(bottom))
            {
                ballYSpeed*= -1;

                if (ballYSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(5, 8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(-11, -8);
                    }
                }
            }

            if (platform.IntersectsWith(left) || platform.IntersectsWith(right))
            {
                ballXSpeed*= -1;

                if (ballXSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(5, 8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballXSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballXSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballXSpeed = randGen.Next(-11, -8);
                    }
                }
            }

            if (platform.IntersectsWith(bottom))
            {
                ballYSpeed*= -1;

                if (ballYSpeed > 0)
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(5, 8);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(7, 10);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(9, 12);
                    }
                }
                else
                {
                    if (difficulty == "easy")
                    {
                        ballYSpeed = randGen.Next(-7, -4);
                    }
                    else if (difficulty == "medium")
                    {
                        ballYSpeed = randGen.Next(-9, -6);
                    }
                    else
                    {
                        ballYSpeed = randGen.Next(-11, -8);
                    }
                }
            }
            
        }

        //part 1 of returning the game to brick-breaking from the lyric guess page
        public void ReturnToBrickBreak1()
        {
            lyricInput.Text = "";
            lyricInput.Enabled = false;
            lyricInput.Visible = false;
            enterButton.Enabled = false;
            enterButton.Visible = false;
        }

        //part 2 of the above
        public void ReturnToBrickBreak2()
        {
            introLabel.Refresh();
            Thread.Sleep(1000);
            introLabel.Text = "";
            gameTimer.Enabled = true;
            status = "brick-breaking";
            bonusCompleted += 1;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                //see if we can get this to work later
                case Keys.Escape:
                    if (status == "start" || status == "end")
                    {
                        Application.Exit();
                    }
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (status == "brick-breaking")
            {
                for (int i = 0; i < brownBricks.Length; i++)
                {
                    e.Graphics.FillRectangle(brownBrush, brownBricks[i]);
                }

                for (int i = 0; i < greenBricks.Length; i++)
                {
                    e.Graphics.FillRectangle(greenBrush, greenBricks[i]);
                }

                for (int i = 0; i < yellowBricks.Length; i++)
                {
                    e.Graphics.FillRectangle(yellowBrush, yellowBricks[i]);
                }

                e.Graphics.FillRectangle(purpleBrush, bonusBrick);

                e.Graphics.FillRectangle(greyBrush, platform);

                e.Graphics.DrawString("Elton John", font, blueBrush, bonusBrick.X+2, bonusBrick.Y+3);

                e.Graphics.FillEllipse(blackBrush, ball);
            }
            
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 6;
            ballYSpeed = -6;
            difficulty = "easy";
            InitializeGame();
        }

        private void mediumButton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 8;
            ballYSpeed = -8;
            difficulty = "medium";
            InitializeGame();
        }

        private void hardButton_Click(object sender, EventArgs e)
        {
            ballXSpeed = 10;
            ballYSpeed = -10;
            difficulty = "hard";
            InitializeGame();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move ball
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            //make ball change direction if it hits a wall
            if (ball.X < 0)
            {
                ballXSpeed *= -1;
            }

            if (ball.Y < 85)
            {
                ballYSpeed *= -1;
            }

            if (ball.X > this.Width - ball.Width)
            {
                ballXSpeed *= -1;
            }

            //reset ball position and subtract a life if ball hits bottom
            if (ball.Y > this.Height - ball.Height)
            {
                lives -= 1;
                ballYSpeed *= -1;
                ball.X = 283;
                ball.Y = 520;
            }

            //move player
            if (leftDown == true && platform.X > 0)
            {
                platform.X -= playerSpeed;
            }

            if (rightDown == true && platform.X < this.Width - platform.Width)
            {
                platform.X += playerSpeed;
            }

            if (ball.IntersectsWith(platform))
            {
                IntersectionDirectionDetection();

            }

            for (int i = 0; i < brownBricks.Length; i++)
            {
                if (ball.IntersectsWith(brownBricks[i]))
                {
                    currentObject = brownBricks[i];
                    IntersectionDirectionDetection();

                    brownBricks[i] = new Rectangle(2000, 2000, 0, 0);
                    score += 10;
                    missingBricks += 1;
                }

            }

            for (int i = 0; i < greenBricks.Length; i++)
            {
                if (ball.IntersectsWith(greenBricks[i]))
                {
                    currentObject = greenBricks[i];
                    IntersectionDirectionDetection();
                    
                    greenBricks[i] = new Rectangle(2000, 2000, 0, 0);
                    score += 10;
                    missingBricks += 1;

                    
                }

            }

            for (int i = 0; i < yellowBricks.Length; i++)
            {
                if (ball.IntersectsWith(yellowBricks[i]))
                {
                    currentObject = yellowBricks[i];
                    IntersectionDirectionDetection();
                    
                    yellowBricks[i] = new Rectangle(2000, 2000, 0, 0);
                    score += 10;
                    missingBricks += 1;

                }


            }

            if (ball.IntersectsWith(bonusBrick))
            {
                missingBricks += 1;
                IntersectionDirectionDetection(); 
                
                bonusBrick = new Rectangle(2000, 2000, 0, 0);
                score += 10;
                status = "lyric guess";


            }

            if (status == "lyric guess")
            {
                gameTimer.Enabled = false;
                lyricInput.Enabled = true;
                lyricInput.Visible = true;
                enterButton.Enabled = true;
                enterButton.Visible = true;

                switch (bonusCompleted)
                {
                    case 0:
                        introLabel.Text = $"Enter the name of the song that contains this lyric: \n'The spotlight's hitting something that's been known to change the weather'";
                        break;
                    case 1:
                        introLabel.Text = $"Enter the name of the song that contains this lyric: \n'It's seven o'clock and I want to rock'";
                        break;
                    case 2:
                        introLabel.Text = $"Enter the name of the song that contains this lyric: \n'Never knowing who to cling to when the rain set in'";
                        break;
                    default:
                        bonusCompleted = 0;
                        break;
                }
                
                
            }

            if (missingBricks == 30 && status == "brick-breaking")
            {
                ResetBricks();
            }

            if (lives == 0)
            {
                if (status != "lyric guess")
                {
                    status = "end";

                    stopwatch.Stop();

                    string time = stopwatch.Elapsed.ToString(@"mm\:ss");

                    gameTimer.Enabled = false;

                    introLabel.Text = $"GAME OVER\n\nScore: {score}\nBonus Bricks Collected: {bonusWon}\nPlay Time: {time}";

                }
            }

            livesLabel.Text = $"Lives: {lives}";
            scoreLabel.Text = $"Score: {score}"; 

            Refresh();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (status == "lyric guess")
            {
                switch (bonusCompleted)
                {
                    case 0:
                        if (lyricInput.Text == "Bennie and the Jets")
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "CORRECT";
                            score *= 2;
                            bonusWon += 1;
                            ReturnToBrickBreak2();
                        }
                        else
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "INCORRECT";
                            score /= 2;
                            ReturnToBrickBreak2();
                            
                        }
                        break;
                    case 1:
                        if (lyricInput.Text == "Saturday Night's Alright for Fighting")
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "CORRECT";
                            score *= 2;
                            bonusWon += 1;
                            ReturnToBrickBreak2();
                        }
                        else
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "INCORRECT";
                            score /= 2;
                            ReturnToBrickBreak2();
                        }
                        break;
                    case 2:
                        if (lyricInput.Text == "Candle in the Wind")
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "CORRECT";
                            score *= 2;
                            bonusWon += 1;
                            ReturnToBrickBreak2();
                        }
                        else
                        {
                            ReturnToBrickBreak1();
                            introLabel.Text = "INCORRECT";
                            score /= 2;
                            ReturnToBrickBreak2();
                        }
                        break;
                }
               
            }
        }
    }
}
