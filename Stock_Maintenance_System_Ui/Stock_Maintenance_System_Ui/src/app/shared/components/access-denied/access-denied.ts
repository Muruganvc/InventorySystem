import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-access-denied',
  imports: [],
  templateUrl: './access-denied.html',
  styleUrl: './access-denied.scss'
})
export class AccessDenied {

  private readonly router = inject(Router);

  ngOnInit(): void {
    // Prevent back navigation while on this page
    history.pushState(null, '', window.location.href);
    window.onpopstate = () => {
      history.pushState(null, '', window.location.href);
    };
  }

  // Optional navigation away button
  goToHome(): void {
    this.router.navigate(['/dashboard']);
  }
}
