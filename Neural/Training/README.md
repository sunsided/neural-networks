# Network Training

The ``ITraining`` and ``ICostGradient`` interfaces define components for deriving the error gradient of the current network, as well as performing the actual weight updates. If no cost gradient implementation is given to the training class, a default implementation will be used. 

## MomentumDescent

This class implements momentum-based gradient descent. Here the gradient descent algorithm is enhanced by a small momentum term that adds a fraction of the last iteration's delta values to the current iteration's step size in order to better handle areas of small gradients. 