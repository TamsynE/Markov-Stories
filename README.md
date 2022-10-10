# Markov-Stories #

I created a program that generates new and unique stories. But that's not all, it also generates new songs, essays, speeches, and even book reports. All that you, the user, needs to do is feed it some sample text, and then it will generate literature of the same genre. The story generator program is based on a Markov Model. It usually knows how to coherently string a few words together, but it won't keep track of things like context, grammer, or most punctuation.

"Markov Model" is named after a famous mathmatician. It can be used as a powerful tool for things like handwriting recognition, simple weather models, predicting the stock market, data compression, and much more. Quite simply, a Markov Model is a simulation that involves a random variable. The model is mostly concerned with its present environment and has only the faintest idea about what happened in the past.

My Markov Model tracks sequences of characters from the sample text. For each sequence of 'n' characters, the model tracks which characters are most likely to come next.
Once the program is finished with cataloguing the entire text, it generates a story. The Markov Model is seeded with an initial message that is as long, or longer, than the 'n' characters. (This guarantees that the beginning of the story produces characters that can be found in our Markov Model). Using this initial seed, the Markov Model randomly chooses the next letter using the probability distribution that it previously generated. Each time it generates a random letter, it takes the next sequence of 'n' characters and repeats. It does this until the story reaches some predetermined stopping point, likely a maximum character limit.

The generated stories vary greatly depending on the length of the model. A shorter sequence diverges from the original text very quickly, but generates a lot of nonsense. A longer sequence creates a story that is closer to the original text, but it is more likely make sense.
