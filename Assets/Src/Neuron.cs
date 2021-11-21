using UnityEngine;

public class Neuron
{
    //-----------------------------------------------------
    public Neuron[] PreviousLayer;
    public float[] Weights;
    public float Bias;
    public float Value;

    //-----------------------------------------------------
    public Neuron( Neuron[] previousLayer )
    {
        this.PreviousLayer = previousLayer;

        if (previousLayer == null) return;

        this.Weights = new float[ this.PreviousLayer.Length ];
        for (int i = 0; i < this.Weights.Length; i++) {
            this.Weights[ i ] = Random.Range( -1.0f, 1.0f );
        }

        this.Bias = Random.Range( -1.0f, 1.0f );
    }

    //-----------------------------------------------------
    public void CalculateValue()
    {
        this.Value = 0;
        for (int i = 0; i < this.Weights.Length; i++) {
            this.Value += this.Weights[ i ] * this.PreviousLayer[ i ].Value;
        }

        this.Value += this.Bias;

        // Activation function from paper "Learning representations by back-propagating erros" 
        // http://www.cs.toronto.edu/~hinton/absps/naturebp.pdf
        this.Value = 1.0f / (1.0f + Mathf.Exp( -this.Value ));
    }

    //-----------------------------------------------------
}