import { AfterViewInit, Component, OnInit, ViewChild,} from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { NotificationComponent } from './Core/notification/notification.component';
import { NotificationService } from './Core/notification/notification.service';

@Component({
  selector: 'app-root',
  standalone: false,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',

})
export class AppComponent implements AfterViewInit, OnInit {
  title = 'ClentApp';
  @ViewChild(NotificationComponent) notification!: NotificationComponent;

  constructor(public router: Router, private notificationService: NotificationService){}

  ngAfterViewInit() {
    this.notificationService.registerNotificationComponent(this.notification);
  }

  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.clearLocalStorage();
      }
    });
  }

  clearLocalStorage() {
    const navEntry = performance.getEntriesByType("navigation")[0] as PerformanceNavigationTiming;

    if (navEntry && navEntry.type !== 'reload' && navEntry.type !== 'back_forward') {
      localStorage.removeItem('selectedItems');
      localStorage.removeItem('order');
    }
  }
}
