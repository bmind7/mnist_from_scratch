using System;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    //-----------------------------------------------------
    public static App Instance;

    private List<MnistImage> trainDataset;
    private List<MnistImage> testDataset;

    private CpuNet cpuNet;
    private Trainer trainer;
    //-----------------------------------------------------
    private void Awake()
    {
        Instance = this;

        var trainImages = Resources.Load( "train_images" ) as TextAsset;
        var trainLabels = Resources.Load( "train_labels" ) as TextAsset;
        trainDataset = MnistHelper.Load( trainImages.bytes, trainLabels.bytes );

        var testImages = Resources.Load( "test_images" ) as TextAsset;
        var testLabels = Resources.Load( "test_labels" ) as TextAsset;
        testDataset = MnistHelper.Load( testImages.bytes, testLabels.bytes );

        Debug.Log( $"[App]: Loaded trainDataset items - {trainDataset.Count}" );
        Debug.Log( $"[App]: Loaded testDataset items - {testDataset.Count}" );

        this.cpuNet = new CpuNet();
        this.cpuNet.AddLayer( 784 );
        this.cpuNet.AddLayer( 16 );
        this.cpuNet.AddLayer( 16 );
        this.cpuNet.AddLayer( 10 );

        this.trainer = new Trainer( this.cpuNet );
    }
    //-----------------------------------------------------
    public MnistImage GetTrainingItemByIndex( int idx ) => trainDataset[ idx ];
    //-----------------------------------------------------
    public void Predict( int idx )
    {
        float[] result = this.cpuNet.Predict( trainDataset[ idx ].Pixels );
        Debug.Log( $"[App]: network output - {string.Join( ",", result )}" );
    }
    //-----------------------------------------------------
    public void StartTraining()
    {
        
        int epochs = 10;
        Action<float> progressCallback = ( float progress ) =>
        {
            DatasetView.Instance.UpdateTrainingProgress( progress );
        };
        this.StartCoroutine( this.trainer.Train( epochs, this.trainDataset, progressCallback ) );
    }
    //-----------------------------------------------------
}
