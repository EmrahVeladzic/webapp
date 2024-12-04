import { Component,ElementRef ,forwardRef, Input, AfterViewInit , OnInit} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [],
  templateUrl: './input.component.html',
  styleUrl: './input.component.css',
  providers:[
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true,
    },
  ]
})
export class InputComponent implements ControlValueAccessor, AfterViewInit {
  constructor(protected el: ElementRef) {} 



  @Input()  min:number =0;
  @Input()  max:number =100;
  @Input()  default:number =0;
  @Input()  step:number =1;

  protected value:any = 0;
  protected active:boolean=false;

  protected valueSubject = new BehaviorSubject<any>(this.value);
  public valueChanges$ = this.valueSubject.asObservable();
  

  protected onChange: (value: number) => void = () => {};
  protected onTouched: () => void = () => {};


  public setValue(v:any):void{
    this.value=v;
    this.validateInput();
  }
  public getValue():any{
    return this.value;
  }

  registerOnChange(fn: (value: number) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }


  writeValue(v: any): void {
    if (v !== undefined) {
      this.value = v;      
      this.validateInput();
    }
  }

  protected  validateInput():void{
    this.onChange(this.value);
    this.valueSubject.next(this.value);
  }

  ngOnInit(){

    if(this.step<1){
      this.step=1;
    }

    if(this.max<=this.min){
      this.max=this.min+this.step;
    }

    if(this.default<this.min){
      this.default=this.min;
    }
    else if(this.default>this.max){
      this.default=this.max;
    }
    this.value=this.default;
  }

  ngAfterViewInit(){

  }
}
