import { Component, ElementRef, HostListener, ViewChild,Input, forwardRef, AfterViewInit } from '@angular/core';
import { InputComponent } from '../input/input.component';
import {NG_VALUE_ACCESSOR } from '@angular/forms';



@Component({
  selector: 'app-slider',
  standalone: true,
  imports: [],
  templateUrl: './slider.component.html',
  styleUrl: './slider.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SliderComponent),
      multi: true,
    },
  ],
})
export class SliderComponent extends InputComponent implements AfterViewInit {
  @ViewChild('thumb',{static:false})thumb!:ElementRef;
  @ViewChild('left',{static:false})left!:ElementRef;
  @ViewChild('right',{static:false})right!:ElementRef;




  protected override value: number = 0;

  private beginX:number = 0;
  private sliderWidth:number=0;
    
  private min:number = 0;
  @Input() max:number = 100;

  public override setValue(v:number):void{
    super.setValue(v);
    this.validateInput();
  }
  public override getValue():number {
    return this.value;
  }

 
  protected override validateInput():void{
    if(this.value<this.min){
      this.value=this.min;
     }
     else if(this.value>this.max){
      this.value=this.max;
     }
     else{
      this.value=Math.round(this.value);
     }
     var dispValue = (this.value / this.max)*80;
     
     this.left.nativeElement.style.width = `${dispValue}%`; 
     this.right.nativeElement.style.width = `${80 - dispValue}%`;

      if(this.active){
        this.onChange(this.value);
        this.valueSubject.next(this.value);
      }
        
  }

  @HostListener('document:mousedown', ['$event'])
  onMouseDown($event: MouseEvent): void {    
    if ($event.target === this.thumb.nativeElement) {
      this.sliderWidthReset();
      this.active = true;
      this.beginX = $event.clientX;
      $event.preventDefault();
      this.onTouched();
    }
 
  }

  @HostListener('document:mousemove', ['$event'])
  onMouseMove($event: MouseEvent): void {
    if (this.active) {
      const mouseMovement = $event.clientX - this.beginX;   
      const deltaX = (mouseMovement / this.sliderWidth) * (this.max - this.min);  
      this.beginX = $event.clientX;
       this.beginX = $event.clientX;
       this.value += deltaX;
      this.validateInput();
    }
  }

  @HostListener('document:mouseup')
  onMouseUp(): void {
    this.active = false;
  }

  public sliderWidthReset():void{
    this.sliderWidth = this.el.nativeElement.offsetWidth;
  }


  ngAfterViewInit() {
   this.sliderWidthReset();
  }

  @HostListener('window:resize')
  onResize() {
    this.sliderWidthReset();
  }

  override writeValue(v: number): void {
    this.value=v;
    if(this.left&&this.right&&this.thumb){
      this.validateInput();
    }
    
  }

  

}
