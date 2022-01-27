import { UnityEngine } from "csharp";
import { $typeof } from "puerts";
import { UITransBinder } from "./UITransBinder";

export class UIComponent {

    public name: string;
    public transBinder: UITransBinder;
    private _gameObject: UnityEngine.GameObject;
    private _rectTransform: UnityEngine.RectTransform;
    
    //
    private _compCanvas: UnityEngine.Canvas;
    private _compGraphic: UnityEngine.UI.Graphic;
    private _compSelectable: UnityEngine.UI.Selectable;
    private _compText: UnityEngine.UI.Text;
    private _compImage: UnityEngine.UI.Image;
    private _compButton: UnityEngine.UI.Button;
    private _compToggle: UnityEngine.UI.Toggle;
    private _compSlider: UnityEngine.UI.Slider;
    private _compScrollBar: UnityEngine.UI.Scrollbar;
    private _compScrollRect: UnityEngine.UI.ScrollRect;
    private _compInputField: UnityEngine.UI.InputField;

    constructor(rootTrans: UnityEngine.Transform, name: string, ...args: any) {
        this.name = name;
        this.transBinder = new UITransBinder();
        this._gameObject = this.transBinder.GetGOBind(name);
        this._rectTransform = this.transBinder.GetTransBind(name);
    }

    public get transform(): UnityEngine.RectTransform {
        return this._rectTransform;
    }

    public get gameObject(): UnityEngine.GameObject {
        return this._gameObject;
    }

    //Canvas
    private get compCanvas(): UnityEngine.Canvas {
        if (this._compCanvas == null) {
            this._compCanvas = this.GetComponent($typeof(UnityEngine.Canvas)) as UnityEngine.Canvas;
        }
        return this._compCanvas;
    }

    //Graphic
    private get compGraphic(): UnityEngine.UI.Graphic {
        if (this._compGraphic == null) {
            this._compGraphic = this.GetComponent($typeof(UnityEngine.UI.Graphic)) as UnityEngine.UI.Graphic;
        }
        return this._compGraphic;
    }

    public set color(color: UnityEngine.Color) {
        this.compGraphic.color = color;
    }

    public get color(): UnityEngine.Color {
        return this.compGraphic.color
    }

    //Selectable
    private get compSelectable(): UnityEngine.UI.Selectable {
        if (this._compSelectable == null) {
            this._compSelectable = this.GetComponent($typeof(UnityEngine.UI.Selectable)) as UnityEngine.UI.Selectable;
        }
        return this._compSelectable
    }

    //Text
    private get compText(): UnityEngine.UI.Text {
        if (this._compText == null) {
            this._compText = this.GetComponent($typeof(UnityEngine.UI.Text)) as UnityEngine.UI.Text;
        }
        return this._compText;
    }

    public set text(text: string) {
        this.compText.text = text;
    }

    public get text(): string {
        return this.compText.text;
    }

    //Image
    private get compImage(): UnityEngine.UI.Image {
        if (this._compImage == null) {
            this._compImage = this.GetComponent($typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image;
        }
        return this._compImage;
    }

    public set sprite(sprite: UnityEngine.Sprite) {
        this.compImage.sprite = sprite;
    }

    public get sprite(): UnityEngine.Sprite {
        return this.compImage.sprite;
    }

    //Button
    private get compButton(): UnityEngine.UI.Button {
        if (this._compButton == null) {
            this._compButton = this.GetComponent($typeof(UnityEngine.UI.Button)) as UnityEngine.UI.Button;
        }
        return this._compButton;
    }

    public AddButtonListener(eventListener: () => void) {
        this.compButton.onClick.AddListener(eventListener);
    }

    public ButtonInvoke() {
        this.compButton.onClick.Invoke();
    }

    //Toggle
    private get compToggle(): UnityEngine.UI.Toggle {
        if (this._compToggle == null) {
            this._compToggle = this.GetComponent($typeof(UnityEngine.UI.Toggle)) as UnityEngine.UI.Toggle;
        }
        return this._compToggle;
    }

    public AddToggleListener(eventListener: (a: boolean) => void) {
        this.compToggle.onValueChanged.AddListener(eventListener);
    }

    public ToggleInvoke(active: boolean) {
        this.compToggle.onValueChanged.Invoke(active);
    }

    public set isOn(isOn: boolean) {
        this.compToggle.isOn = isOn;
    }

    public get isOn(): boolean {
        return this.compToggle.isOn;
    }

    //Slider
    private get compSlider(): UnityEngine.UI.Slider {
        if (this._compSlider == null) {
            this._compSlider = this.GetComponent($typeof(UnityEngine.UI.Slider)) as UnityEngine.UI.Slider;
        }
        return this._compSlider;
    }

    public set sliderValue(value: number) {
        this.compSlider.value = value;
    }

    public get sliderValue(): number {
        return this.compSlider.value;
    }

    //ScrollBar
    private get compScrollBar(): UnityEngine.UI.Scrollbar {
        if (this._compScrollBar == null) {
            this._compScrollBar = this.GetComponent($typeof(UnityEngine.UI.Scrollbar)) as UnityEngine.UI.Scrollbar;
        }
        return this._compScrollBar;
    }

    public set barValue(value: number) {
        this.compScrollBar.value = value;
    }

    public get barValue(): number {
        return this.compScrollBar.value;
    }

    //ScrollRect


    //InputField
    private get compInputField(): UnityEngine.UI.InputField {
        if (this._compInputField == null) {
            this._compInputField = this.GetComponent($typeof(UnityEngine.UI.InputField)) as UnityEngine.UI.InputField;
        }
        return this._compInputField;
    }

    public set inputText(text: string) {
        this.compInputField.text = text;
    }

    public get inputText(): string {
        return this.compInputField.text;
    }

    //Common Function
    public SetActive(active: boolean) {
        this._gameObject.SetActive(active);
    }

    public GetComponent(type: any):any {
        return this._gameObject.GetComponent($typeof(type))
    }

    public SetSizeWithCurrentAnchors(axis: UnityEngine.RectTransform.Axis, size: number) {
        this._rectTransform.SetSizeWithCurrentAnchors(axis, size);
    }

    //
    private ClearButtonListener() {
        
    }

    private ClearToggleListener() {
        
    }

    public Destroy() {
        this._gameObject = null;
        this._rectTransform = null;
        this._compCanvas = null
        this._compGraphic = null;
        this._compText = null;
        this._compImage = null;
        this._compButton = null;
        this._compToggle = null;
        this._compSlider = null;
        this._compScrollBar = null;
        this._compScrollRect = null;
        this._compInputField = null;
    }
}