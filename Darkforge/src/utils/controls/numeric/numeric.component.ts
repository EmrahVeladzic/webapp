import { Component, ViewChild ,ElementRef, Input, forwardRef} from '@angular/core';
import { InputComponent } from '../input/input.component';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-numeric',
  standalone: true,
  imports: [],
  templateUrl: './numeric.component.html',
  styleUrl: './numeric.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumericComponent),
      multi: true,
    },
  ],
})
export class NumericComponent extends InputComponent {
  @ViewChild('input',{static:false})input!:ElementRef;


  protected override value: number = 0;
  protected past_value : number = 0;

  @Input()  min:number =0;
  @Input()  max:number =100;

  public step:number = 1;

  protected override validateInput(): void {
       
    if(this.value>this.max){
      this.value=this.max;
    }
    else if(this.value<this.min){
      this.value=this.min;
    }
    this.past_value=this.value;
    
    if(this.active){
      this.onChange(this.value);
      this.valueSubject.next(this.value);
    }
   

  }

  ngAfterViewInit(){
    this.input.nativeElement.value=this.past_value;
  }

  public override setValue(v:any): void {
    if(Number.parseInt(v)){
      this.value = Math.round(v);
      this.active=true;
      this.validateInput();
      this.active=false;
    }
    this.input.nativeElement.value=this.past_value;
  }

  public increment_decrement(i_d:number){
    this.value+=i_d;
    this.active=true;
    this.validateInput();
    this.active=false;
    this.input.nativeElement.value=this.past_value;
  }

  override writeValue(v: number): void {
    if(this.input){
      this.value=v;
      this.validateInput();
      this.input.nativeElement.value=this.past_value;
    }
  }


 

}
