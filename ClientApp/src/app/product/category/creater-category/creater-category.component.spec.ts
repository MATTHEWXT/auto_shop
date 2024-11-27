import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreaterCategoryComponent } from './creater-category.component';

describe('CreaterCategoryComponent', () => {
  let component: CreaterCategoryComponent;
  let fixture: ComponentFixture<CreaterCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreaterCategoryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreaterCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
