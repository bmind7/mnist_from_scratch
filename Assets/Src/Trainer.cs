using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trainer
{
    //-----------------------------------------------------
    public const float SECONDS_BETWEEN_UI_UPDATES = 1.0f;

    //-----------------------------------------------------
    private CpuNet network;
    //-----------------------------------------------------
    public Trainer( CpuNet network )
    {
        this.network = network;
    }
    //-----------------------------------------------------
    public IEnumerator Train( int epochs,
                              List<MnistImage> dataset,
                              Action<float> progressCallback,
                              float datasetSplit = 0.8f )
    {
        double timeOfNextUpdate = Time.realtimeSinceStartupAsDouble + SECONDS_BETWEEN_UI_UPDATES;

        int splitPoint = (int)(dataset.Count * datasetSplit);
        var trainIndices = Enumerable.Range( 0, splitPoint ).ToList();
        var validIndices = Enumerable.Range( splitPoint, dataset.Count - splitPoint ).ToList();

        this.CalculateError( validIndices, dataset );

        var rand = new System.Random( Seed: 293842 );
        for (int epoch = 0; epoch < epochs; epoch++) {
            // Shuffle training set
            trainIndices.Sort( ( left, right ) => rand.Next( -1, 1 ) );

            for (int i = 0; i < trainIndices.Count; i++) {
                var mnistImage = dataset[ trainIndices[ i ] ];
                float[] predictions = this.network.Predict( mnistImage.Pixels );

                this.network.BackPropagation( mnistImage.Label, predictions );

                // Dont update UI every frame, it will stlow down training process
                if (Time.realtimeSinceStartupAsDouble > timeOfNextUpdate){
                    timeOfNextUpdate = Time.realtimeSinceStartup + SECONDS_BETWEEN_UI_UPDATES;

                    progressCallback?.Invoke( (float)i / trainIndices.Count );

                    yield return null;
                }
            }
            this.CalculateError( validIndices, dataset );
        }
    }
    //-----------------------------------------------------
    private void CalculateError( List<int> validIndices, List<MnistImage> dataset )
    {
        float accumError = 0;
        int accurateCount = 0;
        for (int i = 0; i < validIndices.Count; i++) {
            var mnistImage = dataset[ validIndices[ i ] ];
            float[] predictions = this.network.Predict( mnistImage.Pixels );
            for (int j = 0; j < predictions.Length; j++) {
                accumError += Mathf.Pow( (predictions[ j ] - mnistImage.Label[ j ]), 2.0f );
            }

            if (this.IsAccurate( mnistImage.Label, predictions )) {
                accurateCount++;
            }
        }
        int outputSize = App.Instance.GetTrainingItemByIndex( 0 ).Label.Length;
        accumError /= (float)(validIndices.Count * outputSize);

        Debug.Log( $"[Trainer] MSE: {accumError}" );
        Debug.Log( $"[Trainer] Accuracy: {(float)accurateCount / validIndices.Count}" );
    }
    //-----------------------------------------------------
    private bool IsAccurate( float[] label, float[] prediction )
    {
        int maxIdx = 0;
        for (int i = 1; i < prediction.Length; i++) {
            if (prediction[ i ] > prediction[ maxIdx ]) {
                maxIdx = i;
            }
        }

        return Mathf.Approximately( label[ maxIdx ], 1.0f );
    }
    //-----------------------------------------------------
}
