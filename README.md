# MNIST-CNN
A Windows Forms App of a LeNet-5 CNN with user specific variables to recognise the MNIST handwritten characters

## Description
The program reads in the MNIST data from "mnist_train.csv" and "mnist_test.csv" in the folder of the executable
* The [START] button will perform the following:
  > If it is the 1st time, it will perfrom INITIAL TRAINING on the TRAINING DATA, followed by TESTING on the TEST DATA

  > It will perfrom the next EPOCH, followed by TESTING on the TEST DATA

  > if [Train Until Maximum Epochs] is unchecked, it will perfrom until the max EPOCH or until Testing Accuracy starts decreasing

* The [STOP] button will signal the program to end the current task

* The [UPDATE] button will take the user input values for the changable variables and update them
---
* The USER can change the following parameters of the MNIST CNN
  > Number of nodes in the Hidden Layer  (10 - 100)

  > Learning Rate                        (0.0 - 1.0)

  > Maximum Epochs                       (1 - 100)
---
* There are 3 checkboxs that the USER can select
  > Train Until Maximum Epochs:  The training will continue until the USER selected "Maximum Epochs" is reached regardless of Testing Accuracy

  > Display Output Every 1000:  The output of [0 - 9] will be shown, with the actual label being colored green, followed by the current accuracy of the task

  > Display Progress Output:  The current progress within the data is displayed every 100 characters


The C# code is in Form1.cs

The MNIST data is taken from "https://pjreddie.com/projects/mnist-in-csv/"
