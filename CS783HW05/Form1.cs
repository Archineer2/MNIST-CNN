// MNIST CNN Program by Ryan Desacola
// CS 783 - 1001
// 10/16/2020
//
// The program reads in the MNIST data from "mnist_train.csv" and "mnist_test.csv"
// * The [START] button will perform the following:
//   > If it is the 1st time, it will perfrom INITIAL TRAINING on the TRAINING DATA, followed by TESTING on the TEST DATA
//   > It will perfrom the next EPOCH, followed by TESTING on the TEST DATA
//   > if [Train Until Threshold] is checked, it will perfrom EPOCHs until Training Accuracy > Threshold, or Training Accuracy starts decreasing
//
// * The [STOP] button will signal the program to end the current task
//
// * The [UPDATE] button will take the user input values for the changable variables and update them
//
// * The USER can change the following parameters of the MNIST ANN
//   > Number of nodes in the Hidden Layer  (10 - 100)
//   > Learning Rate                        (0.0 - 1.0)
//   > Testing Accuracy Threshold           (0.0 - 100.0)
//
// * There are 2 checkboxs that the USER can select
//   > Perform Next Immediate Epoch
//   > Display Output Every 1000
//     - The output of [0 - 9] will be shown, with the actual label being colored green, followed by the current accuracy of the task

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CS783HW05
{
    public partial class Form1 : Form
    {
        // ******************************************************************************************************************
        // CONSTANTS AND VARIABLES for READING MNIST DATASET
        // ******************************************************************************************************************

        // Constants for # of images in training/testing sets, and # of pixels in a 28x28 image
        const int TRAIN_MAX = 60000;
        const int TEST_MAX = 10000;
        const int DATA_DIM_X = 28;
        const int DATA_DIM_Y = 28;

        // Arrays for training data: Labels (Actual Values) and Data (Pixel values from [0 - 255])
        int[] trainLabels = new int[TRAIN_MAX];
        int[,,] trainData = new int[TRAIN_MAX, DATA_DIM_Y, DATA_DIM_X];

        // Arrays for testing data: Labels (Actual Values) and Data (Pixel values from [0 - 255])
        int[] testLabels = new int[TEST_MAX];
        int[,,] testData = new int[TEST_MAX, DATA_DIM_Y, DATA_DIM_X];

        // ******************************************************************************************************************
        // CONSTANTS AND VARIABLES for MNIST CONVOLUTIONAL NEURAL NETWORK
        // ******************************************************************************************************************

        // "CONSTANTS" FOR MNIST DIMENTIONS (Constants for CNN, Variables for Form1)
        const int FEATURE_MAP = 6;                          // # of Feature Maps & Max Pooling Maps
        const int W01_DIM_Y = 5;                            // Y-Dimension of Weight_01
        const int W01_DIM_X = 5;                            // X-Dimension of Weight_01
        const int FM_DIM_Y = DATA_DIM_Y - W01_DIM_Y + 1;    // Y-Dimension of Feature Map
        const int FM_DIM_X = DATA_DIM_X - W01_DIM_X + 1;    // X-Dimension of Feature Map
        const int MP_DIM_Y = FM_DIM_Y / 2;                  // Y-Dimension of Max Pooling
        const int MP_DIM_X = FM_DIM_X / 2;                  // X-Dimension of Max Pooling
        int HIDDEN_LAYER = 60;                              // # of Nodes in Hidden Layer
        const int OUTPUT_LAYER = 10;                        // # of Nodes in Output Layer

        double ETA = 0.1;                       // Learning Rate (Based on Activation Function(s) - Sigmoid, Sigmoid, SoftMax)

        // ARRAYS FOR CNN (Created at the start of InitializeData())
        // * Feature Map Arrays
        double[,,,] weight_01;                  // [ #_FM, y, x, { old, new, error } ]
        double[,] bias_01;                      // [ #_FM, { old, new, error } ]
        double[,,] output_01;                   // [ #_FM, y, x ]

        // * Max Pooling Arrays
        bool[,,,] max_02;                       // [ #_MP, y, x ]
        double[,,] output_02;                   // [ #_MP, y, x ]

        // * Hidden Layer Arrays
        double[,,,,] weight_03;                 // [ #_MP, y, x, #_HL, { old, new, error } ]
        double[,] bias_03;                      // [ #_HL, { old, new, error } ]
        double[] output_03;                     // [ #_HL ]

        // * Output Layer Arrays
        double[,,] weight_04;                   // [ #_OL, #_HL, { old, new, error } ]
        double[,] bias_04;                      // [ #_OL, { old, new, error } ]
        double[] output_04;                     // [ #_OL ]

        // ******************************************************************************************************************
        // EXTRA VARIABLES AND FLAGS for METRICS and PROGRAM STATES
        // ******************************************************************************************************************


        int epoch = 0;                          // Number of epochs performed
        bool initialized = false;               // State of arrays, weights, biases being initialized
        bool started = false;                   // State of having performed the initial training
        bool stop = false;                      // Flag to stop current tasks

        double trainAccuracy = 0.0;             // Accuracy of Training
        double testAccuracy = 0.0;              // Accuracy of Testing

        int epochMax = 20;                      // Maximum Epochs


        public Form1()
        {
            InitializeComponent();

            label01_HL.Text = "" + HIDDEN_LAYER;
            label02_ETA.Text = "" + ETA;
            label03_THR.Text = "" + epochMax;
            label04_EPOCH.Text = "" + epoch;
            label05_TRAIN.Text = "" + trainAccuracy;
            label06_TEST.Text = "" + testAccuracy;
            label07_TASK.Text = "None";
            label08_CURR.Text = "" + 0;

        }

        private void Load_Form1(object sender, EventArgs e)
        {
            // Taken from Microsoft StreamReader Class page
            // * Modified to read lines and parse into array
            try
            {
                // "Create an instance of StreamReader to read from a file."
                // "The using statement also closes the StreamReader."
                using (StreamReader sr1 = new StreamReader("mnist_train.csv"))
                {
                    // Display to console output what file is being read
                    Console.WriteLine("Reading Training Dataset from \"mnist_train.csv\"");

                    // Variable to keep track of image # and the string of input from file
                    int lineNumber = 0;     // Image #
                    string line;            // Next line from "mnist_train.csv"

                    // "Read and display lines from the file until the end of"
                    // "the file is reached."
                    while ((line = sr1.ReadLine()) != null)
                    {
                        // Variables to parse the line of input and store into Data Array
                        int dataIndex = 0;  // Pixel #
                        bool start = true;  // Flag to store the 1st number into Label Array

                        // Parser to get each number from string seperated by ','
                        foreach (var s in line.Split(','))
                        {
                            int num;
                            if (int.TryParse(s, out num))
                            {
                                if (start)
                                {
                                    // Store the 1st into Label Array
                                    trainLabels[lineNumber] = num;
                                    start = false;
                                }
                                else
                                {
                                    // Store the rest into Data Array and inc Pixel #
                                    trainData[lineNumber, (dataIndex / DATA_DIM_Y), (dataIndex % DATA_DIM_X)] = num;
                                    dataIndex++;
                                }
                            }
                        }

                        // Inc Image #
                        lineNumber++;

                        // Display reading progress to console every 1000 images
                        if (lineNumber % 1000 == 0)
                        {
                            Console.WriteLine("( " + lineNumber + " / 60000 )");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // "Let the user know what went wrong."
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }


            // Taken from Microsoft StreamReader Class page
            // * Modified to read lines and parse into array
            try
            {
                // "Create an instance of StreamReader to read from a file."
                // "The using statement also closes the StreamReader."
                using (StreamReader sr2 = new StreamReader("mnist_test.csv"))
                {
                    // Display to console output what file is being read
                    Console.WriteLine("\nReading Training Dataset from \"mnist_test.csv\"");

                    // Variable to keep track of image # and the string of input from file
                    int lineNumber = 0;     // Image #
                    string line;            // Next line from "mnist_train.csv"

                    // "Read and display lines from the file until the end of"
                    // "the file is reached."
                    while ((line = sr2.ReadLine()) != null)
                    {
                        // Variables to parse the line of input and store into Data Array
                        int dataIndex = 0;  // Pixel #
                        bool start = true;  // Flag to store the 1st number into Label Array

                        // Parser to get each number from string seperated by ','
                        foreach (var s in line.Split(','))
                        {
                            int num;
                            if (int.TryParse(s, out num))
                            {
                                if (start)
                                {
                                    // Store the 1st into Label Array
                                    testLabels[lineNumber] = num;
                                    start = false;
                                }
                                else
                                {
                                    // Store the rest into Data Array and inc Pixel #
                                    testData[lineNumber, (dataIndex / DATA_DIM_Y), (dataIndex % DATA_DIM_X)] = num;
                                    dataIndex++;
                                }
                            }
                        }

                        // Inc Image #
                        lineNumber++;

                        // Display reading progress to console every 1000 images
                        if (lineNumber % 1000 == 0)
                        {
                            Console.WriteLine("( " + lineNumber + " / 10000 )");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // "Let the user know what went wrong."
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\nMNIST dataset reading complete.  Opening Form1.");
        }

        private void InitializeDate()
        {
            Console.WriteLine();
            Console.WriteLine("INITIALIZING DATA");

            initialized = true;

            // CREATE THE ARRAYS WITH THE CORRECT DIMENSIONS FOR THE ANN
            // * Feature Map Arrays
            weight_01 = new double[FEATURE_MAP, W01_DIM_Y, W01_DIM_X, 3];
            bias_01 = new double[FEATURE_MAP, 3];
            output_01 = new double[FEATURE_MAP, FM_DIM_Y, FM_DIM_X];

            // * Max Pooling Arrays
            max_02 = new bool[FEATURE_MAP, FM_DIM_Y, FM_DIM_X, 2];
            output_02 = new double[FEATURE_MAP, FM_DIM_Y, FM_DIM_X];

            // * Hidden Layer Arrays
            weight_03 = new double[FEATURE_MAP, FM_DIM_Y, FM_DIM_X, HIDDEN_LAYER, 3];
            bias_03 = new double[HIDDEN_LAYER, 3];
            output_03 = new double[HIDDEN_LAYER];

            // * Output Layer Arrays
            weight_04 = new double[HIDDEN_LAYER, OUTPUT_LAYER, 3];
            bias_04 = new double[OUTPUT_LAYER, 3];
            output_04 = new double[OUTPUT_LAYER];

            // INITIALIZE THE WEIGHTS AND BIASES
            Random r = new Random();
            for (int a = 0; a < FEATURE_MAP; a++)
            {
                for (int d = 0; d < W01_DIM_Y; d++)
                {
                    for (int e = 0; e < W01_DIM_X; e++)
                    {
                        weight_01[a, d, e, 0] = r.NextDouble() - 0.5;
                    }
                }

                bias_01[a, 0] = r.NextDouble() - 0.5;

                for (int f = 0; f < FM_DIM_Y; f++)
                {
                    for (int g = 0; g < FM_DIM_X; g++)
                    {
                        for (int h = 0; h < HIDDEN_LAYER; h++)
                        {
                            weight_03[a, f, g, h, 0] = r.NextDouble() - 0.5;
                        }
                    }
                }
            }

            for (int h = 0; h < HIDDEN_LAYER; h++)
            {
                bias_03[h, 0] = r.NextDouble() - 0.5;

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    weight_04[h, aa, 0] = r.NextDouble() - 0.5;
                }
            }

            for (int aa = 0; aa < OUTPUT_LAYER; aa++)
            {
                bias_04[aa, 0] = r.NextDouble() - 0.5;
            }


        }

        private void InitialTrainingCNN()
        {
            // VARIABLES FOR LOOPING
            // > i      [0, 60000) || [0, 10000)
            // > a      [0, FEATURE_MAP)
            // > b      [0, FM_DIM_Y)
            // > c      [0, FM_DIM_X)
            // > d      [0, W01_DIM_Y)
            // > e      [0, W01_DIM_X)
            // > f      [0, MP_DIM_Y)
            // > g      [0, MP_DIM_X)
            // > h      [0, HIDDEN_LAYER)
            // > aa     [0, OUTPUT_LAYER)

            int incorrect = 0;
            taskProgressBar01.Value = 0;

            if (!initialized)
            {
                InitializeDate();
            }

            Console.WriteLine();
            Console.WriteLine("BEGINNING TRAINING");
            label07_TASK.Text = "Initial Training";
            label08_CURR.Text = "" + 0;

            // BEGIN INITIAL TRAINING OF CNN
            for (int i = 0; i < TRAIN_MAX; i++)
            {
                int[] truth = new int[OUTPUT_LAYER];

                // Initialize the correct truth array based on label
                for (int d = 0; d < OUTPUT_LAYER; d++)
                {
                    truth[d] = 0;               // Set all truth values to 0
                }
                truth[trainLabels[i]] = 1;      // Set the actual label in truth array to 1

                // PERFORM FORWARD CALCULATIONS
                // * Feature Map Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int b = 0; b < FM_DIM_Y; b++)
                    {
                        for (int c = 0; c < FM_DIM_X; c++)
                        {
                            double sum = 0.0;

                            for (int d = 0; d < W01_DIM_Y; d++)
                            {
                                for (int e = 0; e < W01_DIM_X; e++)
                                {
                                    sum += ((trainData[i, (b + d), (c + e)] / 255.0) * weight_01[a, d, e, 0]);
                                }
                            }

                            sum += bias_01[a, 0];

                            output_01[a, b, c] = (Math.Tanh(sum / 2.0) + 1) / 2;
                        }
                    }
                }

                // * Max Pooling Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            //  1, 2        [2f, 2g],   [2f, 2g+1]
                            //  3, 4        [2f+1, 2g], [2f+1, 2g+1]
                            bool indexY = false;
                            bool indexX = false;
                            double max = output_01[a, (2 * f), (2 * g)];

                            if (max < output_01[a, (2 * f), ((2 * g) + 1)])
                            {
                                indexY = false;
                                indexX = true;
                                max = output_01[a, (2 * f), ((2 * g) + 1)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), (2 * g)])
                            {
                                indexY = true;
                                indexX = false;
                                max = output_01[a, ((2 * f) + 1), (2 * g)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), ((2 * g) + 1)])
                            {
                                indexY = true;
                                indexX = true;
                                max = output_01[a, ((2 * f) + 1), ((2 * g) + 1)];
                            }

                            max_02[a, f, g, 0] = indexY;
                            max_02[a, f, g, 1] = indexX;
                            output_02[a, f, g] = max;
                        }
                    }
                }

                // * Hidden Layer Calculations
                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    double sum = 0.0;

                    for (int a = 0; a < FEATURE_MAP; a++)
                    {
                        for (int f = 0; f < MP_DIM_Y; f++)
                        {
                            for (int g = 0; g < MP_DIM_X; g++)
                            {
                                sum += (output_02[a, f, g] * weight_03[a, f, g, h, 0]);
                            }
                        }
                    }

                    sum += bias_03[h, 0];

                    output_03[h] = (Math.Tanh(sum / 2.0) + 1) / 2;
                }

                // * Output Layer Calculations
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    double sum = 0.0;

                    for (int h = 0; h < HIDDEN_LAYER; h++)
                    {
                        sum += (output_03[h] * weight_04[h, aa, 0]);
                    }

                    sum += bias_04[aa, 0];

                    output_04[aa] = Math.Exp(sum);
                }

                double softMaxSum = 0.0;
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    softMaxSum += output_04[aa];
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    output_04[aa] /= softMaxSum;
                }



                // PERFROM BACK PROPAGATION
                double[,,] tempBias_01 = new double[FEATURE_MAP, MP_DIM_Y, MP_DIM_X];
                double[] tempBias_03 = new double[HIDDEN_LAYER];
                double[] tempBias_04 = new double[OUTPUT_LAYER];

                // * Calculate error for bias_04[]
                for (int aa1 = 0; aa1 < OUTPUT_LAYER; aa1++)
                {
                    double temp = 0.0;

                    for (int aa2 = 0; aa2 < OUTPUT_LAYER; aa2++)
                    {
                        if (aa1 == aa2)
                        {
                            temp += ((output_04[aa2] - truth[aa2]) * (output_04[aa2] * (1 - output_04[aa2])));
                        }
                        else
                        {
                            temp += ((output_04[aa2] - truth[aa2]) * ((-1) * output_04[aa1] * output_04[aa2]));
                        }
                    }

                    tempBias_04[aa1] = temp;
                    bias_04[aa1, 2] = tempBias_04[aa1];
                }

                // * Calculate error for weight_04[] and bias_03[]
                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    double sum = 0.0;

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 2] = (tempBias_04[aa] * output_03[h]);
                        sum += (tempBias_04[aa] * weight_04[h, aa, 0]);
                    }

                    tempBias_03[h] = (sum * (output_03[h] * (1 - output_03[h])));
                    bias_03[h, 2] = tempBias_03[h];
                }

                // * Calculate error for weigh_03[] and bias_01[]
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            double sum = 0.0;

                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 2] = (tempBias_03[h] * output_02[a, f, g]);
                                sum += (tempBias_03[h] * weight_03[a, f, g, h, 0]);
                            }

                            tempBias_01[a, f, g] = (sum * (output_02[a, f, g] * (1 - output_02[a, f, g])));
                        }
                    }

                    double sum2 = 0.0;
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {

                            sum2 += tempBias_01[a, f, g];
                        }
                    }
                    bias_01[a, 2] = sum2;
                }

                // * Calculate error for weight_01[]
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            double sum = 0.0;

                            for (int f = 0; f < MP_DIM_Y; f++)
                            {
                                for (int g = 0; g < MP_DIM_X; g++)
                                {
                                    // Input[i, ((( MP_Y * 2 ) + MAX_Y ) + W01_Y ), ((( MP_X * 2 ) + MAX_X ) + W01_X )]
                                    sum += (tempBias_01[a, f, g] * (trainData[i, (((f * 2) + Convert.ToInt32(max_02[a, f, g, 0])) + d), (((g * 2) + Convert.ToInt32(max_02[a, f, g, 1])) + e)] / 255.0));
                                }
                            }

                            weight_01[a, d, e, 2] = sum;
                        }
                    }
                }

                // CALCULATE THE NEW VALUES OF THE WEIGHTS AND BIASES
                // * Calculate new weights:  NEW = OLD - ETA * ERROR
                //   > 0    Old
                //   > 1    New
                //   > 2    Error
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            weight_01[a, d, e, 1] = weight_01[a, d, e, 0] - ETA * weight_01[a, d, e, 2];
                        }
                    }

                    bias_01[a, 1] = bias_01[a, 0] - ETA * bias_01[a, 2];
                }

                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 1] = weight_03[a, f, g, h, 0] - ETA * weight_03[a, f, g, h, 2];
                            }
                        }
                    }
                }

                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    bias_03[h, 1] = bias_03[h, 0] - ETA * bias_03[h, 2];

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 1] = weight_04[h, aa, 0] - ETA * weight_04[h, aa, 2];
                    }
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    bias_04[aa, 1] = bias_04[aa, 0] - ETA * bias_04[aa, 2];
                }

                // UPDATE THE WEIGHTS AND BIASES
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            weight_01[a, d, e, 0] = weight_01[a, d, e, 1];
                        }
                    }

                    bias_01[a, 0] = bias_01[a, 1];
                }

                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 0] = weight_03[a, f, g, h, 1];
                            }
                        }
                    }
                }

                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    bias_03[h, 1] = bias_03[h, 0];

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 0] = weight_04[h, aa, 1];
                    }
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    bias_04[aa, 0] = bias_04[aa, 1];
                }

                taskProgressBar01.Value++;

                // PREDICTED LABEL VS ACTUAL LABEL
                double maxOutput = output_04[0];
                int maxIndex = 0;
                for (int aa = 1; aa < OUTPUT_LAYER; aa++)
                {
                    if (maxOutput < output_04[aa])
                    {
                        maxOutput = output_04[aa];
                        maxIndex = aa;
                    }
                }
                if (maxIndex != trainLabels[i])
                {
                    incorrect++;
                }

                // CONSOLE OUTPUT FOR PROGRESS
                if (((i + 1) % 100 == 0) && OutputCheckBox02.Checked)
                {
                    Console.WriteLine("( " + (i + 1) + " / 60000 ) Completed <Initial Training>");

                    Application.DoEvents();
                }

                // CONSOLE OUTPUT FOR OUTPUT VALUES
                double currAcc = ((i + 1 - incorrect) * 100) / ((double)(i + 1));
                label08_CURR.Text = "" + currAcc;
                if ((i + 1) % 1000 == 0 && OutputCheckBox01.Checked)
                {
                    Console.WriteLine("Actual Value at Location " + (i + 1) + " is " + trainLabels[i]);

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        if (aa == trainLabels[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("        Accuracy (" + (i + 1 - incorrect) + " / " + (i + 1) + "):    " + currAcc);
                    Console.ResetColor();
                }

                if (stop)
                {
                    return;
                }

                Application.DoEvents();
                // END OF i LOOP
            }

            Console.WriteLine();
            Console.WriteLine("INITIAL TRAINING COMPLETE");

            double temp1 = 0.0;
            if (trainAccuracy < ((TRAIN_MAX - incorrect) / 600.0))
            {
                temp1 = ((TRAIN_MAX - incorrect) / 600.0) - trainAccuracy;
                trainAccuracy = ((TRAIN_MAX - incorrect) / 600.0);
            }
            else
            {
                Console.WriteLine("> Training Accuracy Unchanged ( New Accuracy = " + ((TRAIN_MAX - incorrect) / 600.0) + " )");
            }

            Console.WriteLine("The Percent Accuaracy based on the Predicted Label (Max of Output) vs Actual Label =   " + trainAccuracy + "   (+" + temp1 + ")");
            Console.WriteLine("  Number Correctly Classified:    " + (TRAIN_MAX - incorrect));
            Console.WriteLine("  Number Incorrectly Classified:  " + (incorrect));

            label05_TRAIN.Text = "" + trainAccuracy;
            label07_TASK.Text = "None";
            Application.DoEvents();
        }

        private void TestCNN()
        {

            // VARIABLES FOR LOOPING
            // > i      [0, 60000) || [0, 10000)
            // > a      [0, FEATURE_MAP)
            // > b      [0, FM_DIM_Y)
            // > c      [0, FM_DIM_X)
            // > d      [0, W01_DIM_Y)
            // > e      [0, W01_DIM_X)
            // > f      [0, MP_DIM_Y)
            // > g      [0, MP_DIM_X)
            // > h      [0, HIDDEN_LAYER)
            // > aa     [0, OUTPUT_LAYER)

            int incorrect = 0;
            taskProgressBar01.Value = 0;

            if (!initialized)
            {
                InitializeDate();
            }

            Console.WriteLine();
            Console.WriteLine("BEGINNING TESTING");
            label07_TASK.Text = "Testing";
            label08_CURR.Text = "" + 0;

            // BEGIN TESTING OF CNN
            for (int i = 0; i < TEST_MAX; i++)
            {
                int[] truth = new int[OUTPUT_LAYER];

                // Initialize the correct truth array based on label
                for (int d = 0; d < OUTPUT_LAYER; d++)
                {
                    truth[d] = 0;               // Set all truth values to 0
                }
                truth[testLabels[i]] = 1;      // Set the actual label in truth array to 1

                // PERFORM FORWARD CALCULATIONS
                // * Feature Map Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int b = 0; b < FM_DIM_Y; b++)
                    {
                        for (int c = 0; c < FM_DIM_X; c++)
                        {
                            double sum = 0.0;

                            for (int d = 0; d < W01_DIM_Y; d++)
                            {
                                for (int e = 0; e < W01_DIM_X; e++)
                                {
                                    sum += ((testData[i, (b + d), (c + e)] / 255.0) * weight_01[a, d, e, 0]);
                                }
                            }

                            sum += bias_01[a, 0];

                            output_01[a, b, c] = (Math.Tanh(sum / 2.0) + 1) / 2;
                        }
                    }
                }

                // * Max Pooling Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            //  1, 2        [2f, 2g],   [2f, 2g+1]
                            //  3, 4        [2f+1, 2g], [2f+1, 2g+1]
                            bool indexY = false;
                            bool indexX = false;
                            double max = output_01[a, (2 * f), (2 * g)];

                            if (max < output_01[a, (2 * f), ((2 * g) + 1)])
                            {
                                indexY = false;
                                indexX = true;
                                max = output_01[a, (2 * f), ((2 * g) + 1)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), (2 * g)])
                            {
                                indexY = true;
                                indexX = false;
                                max = output_01[a, ((2 * f) + 1), (2 * g)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), ((2 * g) + 1)])
                            {
                                indexY = true;
                                indexX = true;
                                max = output_01[a, ((2 * f) + 1), ((2 * g) + 1)];
                            }

                            max_02[a, f, g, 0] = indexY;
                            max_02[a, f, g, 1] = indexX;
                            output_02[a, f, g] = max;
                        }
                    }
                }

                // * Hidden Layer Calculations
                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    double sum = 0.0;

                    for (int a = 0; a < FEATURE_MAP; a++)
                    {
                        for (int f = 0; f < MP_DIM_Y; f++)
                        {
                            for (int g = 0; g < MP_DIM_X; g++)
                            {
                                sum += (output_02[a, f, g] * weight_03[a, f, g, h, 0]);
                            }
                        }
                    }

                    sum += bias_03[h, 0];

                    output_03[h] = (Math.Tanh(sum / 2.0) + 1) / 2;
                }

                // * Output Layer Calculations
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    double sum = 0.0;

                    for (int h = 0; h < HIDDEN_LAYER; h++)
                    {
                        sum += (output_03[h] * weight_04[h, aa, 0]);
                    }

                    sum += bias_04[aa, 0];

                    output_04[aa] = Math.Exp(sum);
                }

                double softMaxSum = 0.0;
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    softMaxSum += output_04[aa];
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    output_04[aa] /= softMaxSum;
                }

                taskProgressBar01.Value += 6;

                // PREDICTED LABEL VS ACTUAL LABEL
                double maxOutput = output_04[0];
                int maxIndex = 0;
                for (int aa = 1; aa < OUTPUT_LAYER; aa++)
                {
                    if (maxOutput < output_04[aa])
                    {
                        maxOutput = output_04[aa];
                        maxIndex = aa;
                    }
                }
                if (maxIndex != testLabels[i])
                {
                    incorrect++;
                }

                // CONSOLE OUTPUT FOR PROGRESS
                if (((i + 1) % 100 == 0) && OutputCheckBox02.Checked)
                {
                    Console.WriteLine("( " + (i + 1) + " / 10000 ) Completed <Testing>");

                    Application.DoEvents();
                }

                // CONSOLE OUTPUT FOR OUTPUT VALUES
                double currAcc = ((i + 1 - incorrect) * 100) / ((double)(i + 1));
                label08_CURR.Text = "" + currAcc;
                if ((i + 1) % 1000 == 0 && OutputCheckBox01.Checked)
                {
                    Console.WriteLine("Actual Value at Location " + (i + 1) + " is " + testLabels[i]);

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        if (aa == testLabels[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("        Accuracy (" + (i + 1 - incorrect) + " / " + (i + 1) + "):    " + currAcc);
                    Console.ResetColor();
                }

                if(stop)
                {
                    return;
                }

                // END OF i LOOP
                Application.DoEvents();
            }

            Console.WriteLine();
            Console.WriteLine("TESTING COMPLETE");

            double temp1 = 0.0;
            if (testAccuracy < ((TEST_MAX - incorrect) / 100.0))
            {
                temp1 = ((TEST_MAX - incorrect) / 100.0) - testAccuracy;
                testAccuracy = ((TEST_MAX - incorrect) / 100.0);
            }
            else
            {
                Console.WriteLine("> Training Accuracy Unchanged ( New Accuracy = " + ((TEST_MAX - incorrect) / 100.0) + " )");
            }

            Console.WriteLine("The Percent Accuaracy based on the Predicted Label (Max of Output) vs Actual Label =   " + testAccuracy + "   (+" + temp1 + ")");
            Console.WriteLine("  Number Correctly Classified:    " + (TEST_MAX - incorrect));
            Console.WriteLine("  Number Incorrectly Classified:  " + (incorrect));

            label06_TEST.Text = "" + testAccuracy;
            label07_TASK.Text = "None";
            Application.DoEvents();
        }

        private void PerformEpoch()
        {

            // VARIABLES FOR LOOPING
            // > i      [0, 60000) || [0, 10000)
            // > a      [0, FEATURE_MAP)
            // > b      [0, FM_DIM_Y)
            // > c      [0, FM_DIM_X)
            // > d      [0, W01_DIM_Y)
            // > e      [0, W01_DIM_X)
            // > f      [0, MP_DIM_Y)
            // > g      [0, MP_DIM_X)
            // > h      [0, HIDDEN_LAYER)
            // > aa     [0, OUTPUT_LAYER)

            int incorrect = 0;
            taskProgressBar01.Value = 0;

            if (!initialized)
            {
                InitializeDate();
            }

            epoch++;

            Console.WriteLine();
            Console.WriteLine("BEGINNING EPOCH " + epoch);
            label04_EPOCH.Text = "" + epoch;
            label07_TASK.Text = "Epoch " + epoch;
            label08_CURR.Text = "" + 0;

            // BEGIN EPOCH OF CNN
            for (int i = 0; i < TRAIN_MAX; i++)
            {
                int[] truth = new int[OUTPUT_LAYER];

                // Initialize the correct truth array based on label
                for (int d = 0; d < OUTPUT_LAYER; d++)
                {
                    truth[d] = 0;               // Set all truth values to 0
                }
                truth[trainLabels[i]] = 1;      // Set the actual label in truth array to 1

                // PERFORM FORWARD CALCULATIONS
                // * Feature Map Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int b = 0; b < FM_DIM_Y; b++)
                    {
                        for (int c = 0; c < FM_DIM_X; c++)
                        {
                            double sum = 0.0;

                            for (int d = 0; d < W01_DIM_Y; d++)
                            {
                                for (int e = 0; e < W01_DIM_X; e++)
                                {
                                    sum += ((trainData[i, (b + d), (c + e)] / 255.0) * weight_01[a, d, e, 0]);
                                }
                            }

                            sum += bias_01[a, 0];

                            output_01[a, b, c] = (Math.Tanh(sum / 2.0) + 1) / 2;
                        }
                    }
                }

                // * Max Pooling Calculations
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            //  1, 2        [2f, 2g],   [2f, 2g+1]
                            //  3, 4        [2f+1, 2g], [2f+1, 2g+1]
                            bool indexY = false;
                            bool indexX = false;
                            double max = output_01[a, (2 * f), (2 * g)];

                            if (max < output_01[a, (2 * f), ((2 * g) + 1)])
                            {
                                indexY = false;
                                indexX = true;
                                max = output_01[a, (2 * f), ((2 * g) + 1)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), (2 * g)])
                            {
                                indexY = true;
                                indexX = false;
                                max = output_01[a, ((2 * f) + 1), (2 * g)];
                            }

                            if (max < output_01[a, ((2 * f) + 1), ((2 * g) + 1)])
                            {
                                indexY = true;
                                indexX = true;
                                max = output_01[a, ((2 * f) + 1), ((2 * g) + 1)];
                            }

                            max_02[a, f, g, 0] = indexY;
                            max_02[a, f, g, 1] = indexX;
                            output_02[a, f, g] = max;
                        }
                    }
                }

                // * Hidden Layer Calculations
                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    double sum = 0.0;

                    for (int a = 0; a < FEATURE_MAP; a++)
                    {
                        for (int f = 0; f < MP_DIM_Y; f++)
                        {
                            for (int g = 0; g < MP_DIM_X; g++)
                            {
                                sum += (output_02[a, f, g] * weight_03[a, f, g, h, 0]);
                            }
                        }
                    }

                    sum += bias_03[h, 0];

                    output_03[h] = (Math.Tanh(sum / 2.0) + 1) / 2;
                }

                // * Output Layer Calculations
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    double sum = 0.0;

                    for (int h = 0; h < HIDDEN_LAYER; h++)
                    {
                        sum += (output_03[h] * weight_04[h, aa, 0]);
                    }

                    sum += bias_04[aa, 0];

                    output_04[aa] = Math.Exp(sum);
                }

                double softMaxSum = 0.0;
                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    softMaxSum += output_04[aa];
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    output_04[aa] /= softMaxSum;
                }



                // PERFROM BACK PROPAGATION
                double[,,] tempBias_01 = new double[FEATURE_MAP, MP_DIM_Y, MP_DIM_X];
                double[] tempBias_03 = new double[HIDDEN_LAYER];
                double[] tempBias_04 = new double[OUTPUT_LAYER];

                // * Calculate error for bias_04[]
                for (int aa1 = 0; aa1 < OUTPUT_LAYER; aa1++)
                {
                    double temp = 0.0;

                    for (int aa2 = 0; aa2 < OUTPUT_LAYER; aa2++)
                    {
                        if (aa1 == aa2)
                        {
                            temp += ((output_04[aa2] - truth[aa2]) * (output_04[aa2] * (1 - output_04[aa2])));
                        }
                        else
                        {
                            temp += ((output_04[aa2] - truth[aa2]) * ((-1) * output_04[aa1] * output_04[aa2]));
                        }
                    }

                    tempBias_04[aa1] = temp;
                    bias_04[aa1, 2] = tempBias_04[aa1];
                }

                // * Calculate error for weight_04[] and bias_03[]
                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    double sum = 0.0;

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 2] = (tempBias_04[aa] * output_03[h]);
                        sum += (tempBias_04[aa] * weight_04[h, aa, 0]);
                    }

                    tempBias_03[h] = (sum * (output_03[h] * (1 - output_03[h])));
                    bias_03[h, 2] = tempBias_03[h];
                }

                // * Calculate error for weigh_03[] and bias_01[]
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            double sum = 0.0;

                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 2] = (tempBias_03[h] * output_02[a, f, g]);
                                sum += (tempBias_03[h] * weight_03[a, f, g, h, 0]);
                            }

                            tempBias_01[a, f, g] = (sum * (output_02[a, f, g] * (1 - output_02[a, f, g])));
                        }
                    }

                    double sum2 = 0.0;
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {

                            sum2 += tempBias_01[a, f, g];
                        }
                    }
                    bias_01[a, 2] = sum2;
                }

                // * Calculate error for weight_01[]
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            double sum = 0.0;

                            for (int f = 0; f < MP_DIM_Y; f++)
                            {
                                for (int g = 0; g < MP_DIM_X; g++)
                                {
                                    // Input[i, ((( MP_Y * 2 ) + MAX_Y ) + W01_Y ), ((( MP_X * 2 ) + MAX_X ) + W01_X )]
                                    sum += (tempBias_01[a, f, g] * (trainData[i, (((f * 2) + Convert.ToInt32(max_02[a, f, g, 0])) + d), (((g * 2) + Convert.ToInt32(max_02[a, f, g, 1])) + e)] / 255.0));
                                }
                            }

                            weight_01[a, d, e, 2] = sum;
                        }
                    }
                }

                // CALCULATE THE NEW VALUES OF THE WEIGHTS AND BIASES
                // * Calculate new weights:  NEW = OLD - ETA * ERROR
                //   > 0    Old
                //   > 1    New
                //   > 2    Error
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            weight_01[a, d, e, 1] = weight_01[a, d, e, 0] - ETA * weight_01[a, d, e, 2];
                        }
                    }

                    bias_01[a, 1] = bias_01[a, 0] - ETA * bias_01[a, 2];
                }

                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 1] = weight_03[a, f, g, h, 0] - ETA * weight_03[a, f, g, h, 2];
                            }
                        }
                    }
                }

                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    bias_03[h, 1] = bias_03[h, 0] - ETA * bias_03[h, 2];

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 1] = weight_04[h, aa, 0] - ETA * weight_04[h, aa, 2];
                    }
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    bias_04[aa, 1] = bias_04[aa, 0] - ETA * bias_04[aa, 2];
                }

                // UPDATE THE WEIGHTS AND BIASES
                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int d = 0; d < W01_DIM_Y; d++)
                    {
                        for (int e = 0; e < W01_DIM_X; e++)
                        {
                            weight_01[a, d, e, 0] = weight_01[a, d, e, 1];
                        }
                    }

                    bias_01[a, 0] = bias_01[a, 1];
                }

                for (int a = 0; a < FEATURE_MAP; a++)
                {
                    for (int f = 0; f < MP_DIM_Y; f++)
                    {
                        for (int g = 0; g < MP_DIM_X; g++)
                        {
                            for (int h = 0; h < HIDDEN_LAYER; h++)
                            {
                                weight_03[a, f, g, h, 0] = weight_03[a, f, g, h, 1];
                            }
                        }
                    }
                }

                for (int h = 0; h < HIDDEN_LAYER; h++)
                {
                    bias_03[h, 1] = bias_03[h, 0];

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        weight_04[h, aa, 0] = weight_04[h, aa, 1];
                    }
                }

                for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                {
                    bias_04[aa, 0] = bias_04[aa, 1];
                }

                taskProgressBar01.Value++;

                // PREDICTED LABEL VS ACTUAL LABEL
                double maxOutput = output_04[0];
                int maxIndex = 0;
                for (int aa = 1; aa < OUTPUT_LAYER; aa++)
                {
                    if (maxOutput < output_04[aa])
                    {
                        maxOutput = output_04[aa];
                        maxIndex = aa;
                    }
                }
                if (maxIndex != trainLabels[i])
                {
                    incorrect++;
                }

                // CONSOLE OUTPUT FOR PROGRESS
                if (((i + 1) % 100 == 0) && OutputCheckBox02.Checked)
                {
                    Console.WriteLine("( " + (i + 1) + " / 60000 ) Completed <Epoch " + epoch + ">");

                    Application.DoEvents();
                }

                // CONSOLE OUTPUT FOR OUTPUT VALUES
                double currAcc = ((i + 1 - incorrect) * 100) / ((double)(i + 1));
                label08_CURR.Text = "" + currAcc;

                if ((i + 1) % 1000 == 0 && OutputCheckBox01.Checked)
                {
                    Console.WriteLine("Actual Value at Location " + (i + 1) + " is " + trainLabels[i]);

                    for (int aa = 0; aa < OUTPUT_LAYER; aa++)
                    {
                        if (aa == trainLabels[i])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine("    Value of Output " + (aa) + " =   " + output_04[aa]);
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("        Accuracy (" + (i + 1 - incorrect) + " / " + (i + 1) + "):    " + currAcc);
                    Console.ResetColor();
                }

                if (stop)
                {
                    return;
                }

                Application.DoEvents();
                // END OF i LOOP
            }

            Console.WriteLine();
            Console.WriteLine("EPOCH " + epoch + " COMPLETE");

            double temp1 = 0.0;
            if (trainAccuracy < ((TRAIN_MAX - incorrect) / 600.0))
            {
                temp1 = ((TRAIN_MAX - incorrect) / 600.0) - trainAccuracy;
                trainAccuracy = ((TRAIN_MAX - incorrect) / 600.0);
            }
            else
            {
                Console.WriteLine("> Training Accuracy Unchanged ( New Accuracy = " + ((TRAIN_MAX - incorrect) / 600.0) + " )");
            }

            Console.WriteLine("The Percent Accuaracy based on the Predicted Label (Max of Output) vs Actual Label =   " + trainAccuracy + "   (+" + temp1 + ")");
            Console.WriteLine("  Number Correctly Classified:    " + (TRAIN_MAX - incorrect));
            Console.WriteLine("  Number Incorrectly Classified:  " + (incorrect));

            label05_TRAIN.Text = "" + trainAccuracy;
            label07_TASK.Text = "None";
            Application.DoEvents();
        }

        private void StartButton01_Click(object sender, EventArgs e)
        {
            int counter = 0;

            if(stop)
            {
                stop = false;
            }

            if(ThresholdCheckBox01.Checked)
            {
                started = true;
                InitialTrainingCNN();
                TestCNN();
                while(true)
                {
                    PerformEpoch();
                    TestCNN();

                    counter++;

                    if (counter >= epochMax)
                    {
                        stop = true;
                    }

                    if (stop)
                    {
                        Console.WriteLine();
                        Console.WriteLine("CURRENT TASK HAS BEEN STOPPED");
                        label07_TASK.Text = "None";
                        stop = false;
                        return;
                    }
                }
            }
            else
            {
                if (!started)
                {
                    started = true;
                    TestCNN();
                    InitialTrainingCNN();
                    TestCNN();

                    if (stop)
                    {
                        Console.WriteLine();
                        Console.WriteLine("CURRENT TASK HAS BEEN STOPPED");
                        label07_TASK.Text = "None";
                        stop = false;
                        return;
                    }
                }
                else
                {
                    PerformEpoch();
                    TestCNN();

                    if (stop)
                    {
                        Console.WriteLine();
                        Console.WriteLine("CURRENT TASK HAS BEEN STOPPED");
                        label07_TASK.Text = "None";
                        stop = false;
                        return;
                    }
                }
            }
        }

        private void StopButton02_Click(object sender, EventArgs e)
        {
            if (started)
            {
                stop = true;
                return;
            }
        }

        private void UpdateButton03_Click(object sender, EventArgs e)
        {
            if (started)
            {
                stop = true;
            }

            int tempHL = Convert.ToInt32(textBox01_HL.Text);
            double tempETA = Convert.ToDouble(textBox02_ETA.Text);
            int tempMAX = Convert.ToInt32(textBox03_THR.Text);

            if (tempHL < 10 || tempHL > 100)
            {
                Console.WriteLine();
                Console.WriteLine("Error: Number of Hidden Layer Nodes out of range (10 - 100)");
                return;
            }
            if (tempETA < 0.0 || tempETA > 1.0)
            {
                Console.WriteLine();
                Console.WriteLine("Error: Learning Rate out of range (0.00 - 1.00)");
                return;
            }
            if (tempMAX < 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error: Maximum Epochs must be positive");
                return;
            }

            initialized = false;
            started = false;

            HIDDEN_LAYER = tempHL;
            ETA = tempETA;
            epochMax = tempMAX;

            epoch = 0;
            trainAccuracy = 0.0;
            testAccuracy = 0.0;

            label01_HL.Text = "" + HIDDEN_LAYER;
            label02_ETA.Text = "" + ETA;
            label03_THR.Text = "" + tempMAX;
            label04_EPOCH.Text = "" + epoch;
            label05_TRAIN.Text = "" + trainAccuracy;
            label06_TEST.Text = "" + testAccuracy;
            label07_TASK.Text = "None";
            label08_CURR.Text = "" + 0;

            Console.WriteLine();
            Console.WriteLine("VARIABLES HAVE BEEN UPDATED");
        }

        // NEW FUNCTION HERE

    }
}
