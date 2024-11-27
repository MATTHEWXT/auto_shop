import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreaterProductComponent } from './creater-product.component';

describe('CreaterProductComponent', () => {
  let component: CreaterProductComponent;
  let fixture: ComponentFixture<CreaterProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreaterProductComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreaterProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
