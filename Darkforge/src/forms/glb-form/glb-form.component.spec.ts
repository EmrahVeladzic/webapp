import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GlbFormComponent } from './glb-form.component';

describe('GlbFormComponent', () => {
  let component: GlbFormComponent;
  let fixture: ComponentFixture<GlbFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GlbFormComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GlbFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
