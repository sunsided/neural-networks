# Handwritten Digit Classification

This is an implementation of the handwritten digit classification as it is given by Andrew Ng's *Machine Learning* Coursera course. The file ``images.dat.gz`` contains the 5000 pre-labeled images from the Matlab exercise.

Learning parameters are hardcoded for the time being.

## Network structure

The network contains of 400 input units (*20x20 pixels*), 25 hidden units (*5x5 pixels*) and 10 output units (*10 possible digits*). The outputs are interpreted as zero-indexed categorical one-hot, i.e. an output of ``[0 0 1 0 0 0 0 0 0 0]`` means that the image was classified as label ``2``. 

Since the output is sigmoidal, it is likely that more than one output is hot; the index of the largest value then determines the label, whereas the value itself can be considered an amount of certainty. So, ``[0 0 0.9 0 0.2 0 0 0 0 0]`` means that the label is ``2`` and the network is 90% sure of that, but it could also be label ``4`` with 20% certainty.

## Image file format

The images are stored sequentially in the following format:

* Class label: 32bit integer (little endian)
* Minimum pixel value: 4-byte single precision
* Maximum pixel value: 4-byte single precision
* Pixel values: 400 4-bite single precision values

## Network files

Networks can be exported to and loaded from a JSON format that contains the MLP network architecture as well as the input weights and biases. The weights are stored in columnwise order, i.e. all the values of the first row, followed by all of the second row etc.

The file format is currently missing activation functions, so sigmoidal activation is implied for all units, including the outputs.