using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Views;
using System.Collections;
using System;

namespace AndroidSlidingPuzzleCsharp
{
    [Activity (Label = "Android Sliding Puzzle Csharp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {

        #region vars
        Button resetButton;
        GridLayout mainLayout;

        int gameViewWidth;
        int tileWidth;


        ArrayList tilesArr;
        ArrayList coordsArr;


        Point emptySpot;

        #endregion

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            SetContentView (Resource.Layout.Main);

            // make a call to set game view
            setGameView ( );

            makeTilesMethod ( );

            randomizeMethod ( );
        }


        private void makeTilesMethod ( )
        {
            tileWidth = gameViewWidth / 4;

            tilesArr = new ArrayList ( );
            coordsArr = new ArrayList ( );


            int counter = 1;
            for (int h = 0; h < 4; h++)
            {
                for (int v = 0; v < 4; v++)
                {
                    MyTextView textTile = new MyTextView (this);

                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec (h);
                    GridLayout.Spec colSpec = GridLayout.InvokeSpec (v);

                    GridLayout.LayoutParams tileLayourParams = new GridLayout.LayoutParams (rowSpec, colSpec);


                    textTile.Text = counter.ToString ( );
                    textTile.SetTextColor (Color.Black);
                    textTile.TextSize = 40;
                    textTile.Gravity = GravityFlags.Center;

                    tileLayourParams.Width = tileWidth - 10;
                    tileLayourParams.Height = tileWidth - 10;
                    tileLayourParams.SetMargins (5, 5, 5, 5);

                    textTile.LayoutParameters = tileLayourParams;

                    textTile.SetBackgroundColor (Color.Green);

                    Point thisLoc = new Point (v, h);
                    coordsArr.Add (thisLoc);

                    textTile.xPos = thisLoc.X;
                    textTile.yPos = thisLoc.Y;


                    textTile.Touch += TextTile_Touch;


                    tilesArr.Add (textTile);

                    mainLayout.AddView (textTile);

                    counter++;
                }
            }

            mainLayout.RemoveView ((MyTextView)tilesArr[15]);
            tilesArr.RemoveAt (15);
        }




        // x=2, y=3
        // x=3, y=3



        void TextTile_Touch (object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                MyTextView thisTile = (MyTextView)sender;

                float xDif = (float) Math.Pow (thisTile.xPos - emptySpot.X, 2);
                float yDif = (float) Math.Pow ( thisTile.yPos - emptySpot.Y, 2);

                float dist = (float) Math.Sqrt (xDif + yDif);


                if ( dist == 1)
                {
                    // tile can move

                    // memorize where the tile used to be
                    Point curPoint = new Point (thisTile.xPos, thisTile.yPos);

                    // now take the tile to the empty

                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec (emptySpot.Y);
                    GridLayout.Spec colSpec = GridLayout.InvokeSpec (emptySpot.X);

                    GridLayout.LayoutParams newLocParams = new GridLayout.LayoutParams (rowSpec, colSpec);


                    thisTile.xPos = emptySpot.X;
                    thisTile.yPos = emptySpot.Y;

                    newLocParams.Width = tileWidth - 10;
                    newLocParams.Height = tileWidth - 10;
                    newLocParams.SetMargins (5, 5, 5, 5);

                    thisTile.LayoutParameters = newLocParams;

                    emptySpot = curPoint;


                }

            }
        }










        private void randomizeMethod ( )
        {
            ArrayList tempCoords = new ArrayList (coordsArr);

            Random myRand = new Random ( );
            foreach (MyTextView any in tilesArr) // 15 elements
            {
                int randIndex = myRand.Next (0, tempCoords.Count); // 12, 12
                Point thisRandLoc = (Point)tempCoords[randIndex];


                GridLayout.Spec rowSpec = GridLayout.InvokeSpec (thisRandLoc.Y);
                GridLayout.Spec colSpec = GridLayout.InvokeSpec (thisRandLoc.X);

                GridLayout.LayoutParams randLayoutParam = new GridLayout.LayoutParams (rowSpec, colSpec);


                any.xPos = thisRandLoc.X;
                any.yPos = thisRandLoc.Y;

                randLayoutParam.Width = tileWidth - 10;
                randLayoutParam.Height = tileWidth - 10;
                randLayoutParam.SetMargins (5, 5, 5, 5);


                any.LayoutParameters = randLayoutParam;

                tempCoords.RemoveAt (randIndex);
            }

            emptySpot = (Point) tempCoords[0];
        }


        private void setGameView ( )
        {
            resetButton = FindViewById<Button> (Resource.Id.resetButtonId);
            resetButton.Click += resetMethod;

            mainLayout = FindViewById<GridLayout> (Resource.Id.gameGridLayoutId);
            gameViewWidth = Resources.DisplayMetrics.WidthPixels;


            mainLayout.ColumnCount = 4;
            mainLayout.RowCount = 4;

            mainLayout.LayoutParameters = new LinearLayout.LayoutParams (gameViewWidth, gameViewWidth);
            mainLayout.SetBackgroundColor (Color.Gray);
        }

        void resetMethod (object sender, System.EventArgs e)
        {
            randomizeMethod ( );
        }
    }


    class MyTextView : TextView
    {
        Activity myContext;

        public MyTextView (Activity context) : base (context)
        {
            myContext = context;
        }

        public int xPos { set; get; }
        public int yPos { set; get; }
    }

}

