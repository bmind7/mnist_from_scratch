using System.Collections.Generic;
using System.Linq;

public class CpuNet
{
    //-----------------------------------------------------
    private List<Neuron[]> Layers = new List<Neuron[]>();

    private float learningRate = 0.01f;

    //-----------------------------------------------------
    public void AddLayer( int neuronAmount )
    {
        var previousLayer = this.Layers.LastOrDefault();
        var newLayer = new Neuron[ neuronAmount ];
        for (int i = 0; i < neuronAmount; i++) {
            newLayer[ i ] = new Neuron( previousLayer );
        }
        this.Layers.Add( newLayer );
    }
    //-----------------------------------------------------
    public float[] Predict( float[] inputValues )
    {
        var inputLayer = this.Layers[ 0 ];
        for (int neuronIdx = 0; neuronIdx < inputLayer.Length; neuronIdx++) {
            inputLayer[ neuronIdx ].Value = inputValues[ neuronIdx ];
        }

        for (int layerIdx = 1; layerIdx < this.Layers.Count; layerIdx++) {
            var currentLayer = this.Layers[ layerIdx ];
            for (int neuronIdx = 0; neuronIdx < currentLayer.Length; neuronIdx++) {
                currentLayer[ neuronIdx ].CalculateValue();
            }
        }

        return this.ReadOutputLayer();
    }

    //-----------------------------------------------------
    private float[] ReadOutputLayer() => this.Layers.Last().Select( neuron => neuron.Value ).ToArray();

    //-----------------------------------------------------
    // Paper - http://www.cs.toronto.edu/~hinton/absps/naturebp.pdf
    // y - predicted values
    // d - expected values
    public void BackPropagation(float[] d, float[] y){
        for (int layerIdx = this.Layers.Count - 1; layerIdx > 0; layerIdx--){
            var currentLayer = this.Layers[ layerIdx ];
            var previousLayer = this.Layers[ layerIdx - 1 ];

            float[] accumY = new float[ previousLayer.Length ];

            for (int j = 0; j < currentLayer.Length; j++) {
                float[] weights = currentLayer[ j ].Weights;
                float dEdx = y[ j ] * (1 - y[ j ]) * (y[ j ] - d[ j ]);
                for (int i = 0; i < weights.Length; i++) {
                    // Formula 6 & 7 from the paper
                    accumY[ i ] += weights[ i ] * dEdx;
                    float deltaW = previousLayer[ i ].Value * dEdx;
                    weights[ i ] -= this.learningRate * deltaW;

                }
                currentLayer[ j ].Bias -= this.learningRate * dEdx;
            }

            y = new float[ previousLayer.Length ];
            d = new float[ previousLayer.Length ];
            for (int j = 0; j < previousLayer.Length; j++) {
                y[ j ] = previousLayer[ j ].Value;
                d[ j ] = previousLayer[ j ].Value - accumY[ j ];
            }
        }
    }
    //-----------------------------------------------------
}
