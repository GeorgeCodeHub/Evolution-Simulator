using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class WindowGraph : MonoBehaviour
{
    private static WindowGraph instance;

    [SerializeField] private Sprite dotSprite;
    private RectTransform GraphContainer;
    private RectTransform LabelTemplateX;
    private RectTransform LabelTemplateY;
    private RectTransform DashContainer;
    private RectTransform DashTemplateX;
    private RectTransform DashTemplateY;
    private List<GameObject> gameObjectList;
    public List<IGraphVisualObject> graphVisualObjectList;
    private GameObject tooltipGameObject;
    private List<RectTransform> yLabelList;
    public TextMeshProUGUI TraitText;

    // Cached values
    public List<float> valueList;
    private int tabIndex;
    private int localCounter;
    private IGraphVisual lineGraphVisual;
    private int maxVisibleValueAmount;
    private int xDayLimit;
    private Func<float, string> getAxisLabelY;
    private float xSize;
    private bool startYScaleAtZero;

    private void Awake()
    {
        instance = this;

        GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        LabelTemplateX = GraphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        LabelTemplateY = GraphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();
        DashContainer = GraphContainer.Find("DashContainer").GetComponent<RectTransform>();
        DashTemplateX = DashContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        DashTemplateY = DashContainer.Find("DashTemplateY").GetComponent<RectTransform>();
        tooltipGameObject = GraphContainer.Find("Tooltip").gameObject;

        xDayLimit = 0;
        valueList = new List<float> { };
        startYScaleAtZero = false;
        gameObjectList = new List<GameObject>();
        yLabelList = new List<RectTransform>();
        graphVisualObjectList = new List<IGraphVisualObject>();

        if (TimeSpeed.exist)
        {
            tabIndex = 0;
            TabIndex(0);
        }
        else
        {
            tabIndex = 3;
            TabIndex(3);
        }
        
        HideTooltip();
    }

    private void Update()
    {
        if (ChickenStatsUI.statCounter > localCounter)
        {
            TabIndex(tabIndex);
            localCounter = ChickenStatsUI.statCounter;
        }

        if(GameController.dayCounter > localCounter)
        {
            TabIndex(tabIndex);
            localCounter = GameController.dayCounter;
        }

        if(GameController.reset == true)
        {
            localCounter = 0;
        }
    }


    public void TabIndex(int tabIndex = 0)
    {

        
        this.tabIndex = tabIndex;
        switch (tabIndex)
        {
            case 0:
                valueList = GameController.SpeedList;
                TraitText.text = "SPEED";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.blue, new Color(5, 5, 5, 1f));
                break;
            case 1:
                valueList = GameController.SizeList;
                TraitText.text = "SIZE";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.red, new Color(5, 5, 5, 1f));
                break;
            case 2:
                valueList = GameController.QualityList;
                TraitText.text = "QUALITY";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.green, new Color(5, 5, 5, 1f));
                break;
            case 3:
                valueList = ChickenStatsUI.SpeedList;
                TraitText.text = "SPEED";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.blue, new Color(5, 5, 5, 1f));
                break;
            case 4:
                valueList = ChickenStatsUI.SizeList;
                TraitText.text = "SIZE";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.red, new Color(5, 5, 5, 1f));
                break;
            case 5:
                valueList = ChickenStatsUI.CharacterCountList;
                TraitText.text = "NUMBER";
                lineGraphVisual = new LineGraphVisual(GraphContainer, dotSprite, Color.green, new Color(5, 5, 5, 1f));
                break;
            default:
                break;
        }

        if (valueList.Count > 30)
        {
            valueList.RemoveAt(0);
            xDayLimit++;
        }
        ShowGraph(valueList, lineGraphVisual, valueList.Count, (int _i) => "" + (_i + 1), (float _f) => "" + Mathf.RoundToInt(_f));
    }


    public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition)
    {
        instance.ShowTooltip(tooltipText, anchoredPosition);
    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPosition)
    {
        tooltipGameObject.SetActive(true);

        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        Text tooltipUIText = tooltipGameObject.transform.Find("Text").GetComponent<Text>();
        tooltipUIText.text = tooltipText;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(
               tooltipUIText.preferredWidth + textPaddingSize * 2f,
               tooltipUIText.preferredHeight + textPaddingSize * 2f
        );

        tooltipGameObject.transform.Find("Background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        tooltipGameObject.transform.SetAsLastSibling();
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }

    private void HideTooltip()
    {
        tooltipGameObject.SetActive(false);
    }

    //Render graph with lines and values on the x,y axis
    public void ShowGraph(List<float> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1,
                          Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        this.valueList = valueList;
        this.getAxisLabelY = getAxisLabelY;

        if (maxVisibleValueAmount <= 0)
        {
            // Show all if no amount specified
            maxVisibleValueAmount = valueList.Count;
        }
        if (maxVisibleValueAmount > valueList.Count)
        {
            // Validate the amount to show the maximum
            maxVisibleValueAmount = valueList.Count;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList)
        {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();

        //Grab the width and height from the container
        float graphWidth = GraphContainer.sizeDelta.x;
        float graphHeight = GraphContainer.sizeDelta.y;

        CalculateYScale(out float yMinimum, out float yMaximum);

        //Set the distance between each point on the graph 
        xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;
        //Render x Axis
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            //Add data point tooltip
            string tooltipText = getAxisLabelY(valueList[i]);
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipText);
            graphVisualObjectList.Add(graphVisualObject);
            
            //Duplicate the x label template
            RectTransform labelX = Instantiate(LabelTemplateX);
            labelX.SetParent(GraphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -5f);

            if(xDayLimit > 0)
            labelX.GetComponent<Text>().text = getAxisLabelX(i + xDayLimit);
            else
            labelX.GetComponent<Text>().text = getAxisLabelX(i);

            gameObjectList.Add(labelX.gameObject);

            RectTransform dashX = Instantiate(DashTemplateX);
            dashX.SetParent(DashContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);

            gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }


        //Render y Axis
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(LabelTemplateY);
            labelY.SetParent(GraphContainer, false);
            labelY.gameObject.SetActive(true);

            float normalizedvalue = i * 1f / separatorCount;

            labelY.anchoredPosition = new Vector2(-7f, normalizedvalue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedvalue * (yMaximum - yMinimum)));

            yLabelList.Add(labelY);
            gameObjectList.Add(labelY.gameObject);

            RectTransform dashY = Instantiate(DashTemplateY);
            dashY.SetParent(DashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(0f, normalizedvalue * graphHeight);

            gameObjectList.Add(dashY.gameObject);
        }
    }

    private void UpdateValue(int index,int value)
    {
        CalculateYScale(out float yMinimumBefore, out float yMaximumBefore);

        valueList[index] = value;

        float graphWidth = GraphContainer.sizeDelta.x;
        float graphHeight = GraphContainer.sizeDelta.y;

        CalculateYScale(out float yMinimum, out float yMaximum);

        bool yScaleChanged = yMinimumBefore != yMinimum || yMaximumBefore != yMaximum;

        if (!yScaleChanged)
        {
            float xPosition = xSize + index * xSize;
            float yPosition = ((value - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

        //Add data point tooltip
            string tooltipText = "" + Mathf.RoundToInt(value);
            graphVisualObjectList[index].SetGraphVisualObjectInfo(new Vector2(xPosition,yPosition), xSize, tooltipText);
        } else {
            // Y scale changed, update whole graph and y axis labels
            // Cycle through all visible data points
            int xIndex = 0;
            for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i<valueList.Count; i++) {
                float xPosition = xSize + xIndex * xSize;
                float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

                // Add data point visual
                string tooltipText = getAxisLabelY(valueList[i]);
                graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), xSize, tooltipText);

                xIndex++;
            }

            for (int i = 0; i < yLabelList.Count; i++)
            {
                float normalizedValue = i * 1f / yLabelList.Count;
                yLabelList[i].GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            }
        }
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum)
    {
        // Identify y Min and Max values
        yMaximum = valueList[0];
        yMinimum = valueList[0];

        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++)
        {
            float value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;

        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum += (yDifference * 0.2f);
        yMinimum -= (yDifference * 0.2f);

        if (startYScaleAtZero)
        {
            yMinimum = 0f; // Start the graph at zero
        }
    }

    //Interface definition for showing visual for a data point
    public interface IGraphVisual
    {
        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();
    }

    //Represents a single Visual Object in the graph
    public interface IGraphVisualObject
    {
        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();
    }

    private class LineGraphVisual : IGraphVisual
    {
        private readonly RectTransform GraphContainer;
        private readonly Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor)
        {
            GraphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastLineGraphVisualObject = null;
        }

        public void CleanUp()
        {
            lastLineGraphVisualObject = null;
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
        {
            GameObject dotGameObject = CreateDot(graphPosition);


            GameObject dotConnectionGameObject = null;
            if (lastLineGraphVisualObject != null)
            {
                dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, lastLineGraphVisualObject);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);

            lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        //Draw circle/Rectangle for each item on the graph
        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("Circle", typeof(Image));
            gameObject.transform.SetParent(GraphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(20, 20);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);

            // Add Button_UI Component which captures UI Mouse Events
            Button_UI chartButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        //Draw lines between dots
        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
        {
            GameObject gameObject = new GameObject("DotConnection", typeof(Image));

            gameObject.transform.SetParent(GraphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;;
            gameObject.GetComponent<Image>().raycastTarget = false;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);

            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);

            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

            return gameObject;
        }

        public class LineGraphVisualObject : IGraphVisualObject
        {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private readonly GameObject dotGameObject;
            private readonly GameObject dotConnectionGameObject;
            private readonly LineGraphVisualObject lastVisualObject;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, LineGraphVisualObject lastVisualObject)
            {
                this.dotGameObject = dotGameObject;
                this.dotConnectionGameObject = dotConnectionGameObject;
                this.lastVisualObject = lastVisualObject;

                if (lastVisualObject != null)
                {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e)
            {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText)
            {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();

                Button_UI dotButtonUI = dotGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                dotButtonUI.MouseOverOnceFunc = () =>
                {
                    ShowTooltip_Static(tooltipText, graphPosition);
                };

                // Hide Tooltip on Mouse Out
                dotButtonUI.MouseOutOnceFunc = () =>
                {
                    HideTooltip_Static();
                };

                OnChangedGraphVisualObjectInfo?.Invoke(this, EventArgs.Empty);
            }

            public void CleanUp()
            {
                Destroy(dotGameObject);
                Destroy(dotConnectionGameObject);
            }

            public Vector2 GetGraphPosition()
            {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection()
            {
                if (dotConnectionGameObject != null)
                {
                    RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }

        }
    }
}
