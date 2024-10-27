import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WlFormComponent } from './wl-form.component';

describe('WlFormComponent', () => {
  let component: WlFormComponent;
  let fixture: ComponentFixture<WlFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WlFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WlFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
