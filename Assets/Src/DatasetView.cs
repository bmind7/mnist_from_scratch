using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DatasetView : MonoBehaviour
{
    //-----------------------------------------------------
    public static DatasetView Instance;
    [SerializeField] private RawImage imgNumber;
    [SerializeField] private Text txtLabel;
    [SerializeField] private Slider progress;

    private int currentIndex = 0;
    //-----------------------------------------------------
    private void Start()
    {
        Instance = this;
        
        ShowMnistItem();
    }
    //-----------------------------------------------------
    public void ShowNext(){
        currentIndex++;
        ShowMnistItem();
    }
    //-----------------------------------------------------
    public void ShowPrevious() {
        if(currentIndex > 0) currentIndex--;
        ShowMnistItem();
    }
    //-----------------------------------------------------
    public void ShowMnistItem()
    {
        MnistImage image = App.Instance.GetTrainingItemByIndex( this.currentIndex );

        var texture = new Texture2D( 28, 28, TextureFormat.RGB24, 1, false );
        texture.filterMode = FilterMode.Point;
        texture.SetPixels( image.GetColorsForTexture() );
        texture.Apply();

        imgNumber.texture = texture;
        txtLabel.text = image.Label.ToList().IndexOf( 1.0f ).ToString();
    }
    //-----------------------------------------------------
    public void Predict(){
        App.Instance.Predict( this.currentIndex );
    }
    //-----------------------------------------------------
    public void Train(){
        App.Instance.StartTraining();
    }
    //-----------------------------------------------------
    public void UpdateTrainingProgress(float value){
        this.progress.value = value;
    }
    //-----------------------------------------------------
}
