import { Component } from '@angular/core';

@Component({
  selector: 'app-empty-page',
  templateUrl: './empty-page.component.html',
  styleUrl: './empty-page.component.css'
})
export class EmptyPageComponent {
  title = 'app';
  showUnauthorizedMessage: boolean = true;
}
