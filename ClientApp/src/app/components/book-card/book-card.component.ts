import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book.model';

@Component({
  selector: 'app-book-card',
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.css']
})
export class BookCardComponent {
@Input() book !: Book

  constructor(
    private router: Router
  ){}
viewDetails(){
this.router.navigate([`books/view/${this.book.id}`])
}
}
