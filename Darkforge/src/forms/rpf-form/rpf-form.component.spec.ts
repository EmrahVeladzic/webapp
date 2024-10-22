import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RpfFormComponent } from './rpf-form.component';

describe('RpfFormComponent', () => {
  let component: RpfFormComponent;
  let fixture: ComponentFixture<RpfFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RpfFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RpfFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
