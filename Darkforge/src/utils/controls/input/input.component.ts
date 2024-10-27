import { Component,ElementRef ,forwardRef} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';


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
export class InputComponent implements ControlValueAccessor {

  constructor(protected el: ElementRef) {} 

  protected value:any = 0;

  protected onChange: (value: number) => void = () => {};
  protected onTouched: () => void = () => {};


  public setValue(v:any):void{
    this.value=v;
    this.onChange(v);
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
      
    }
  }

  
}
