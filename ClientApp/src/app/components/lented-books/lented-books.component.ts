import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book.model';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';

@Component({
  selector: 'app-lented-books',
  templateUrl: './lented-books.component.html',
  styleUrls: ['./lented-books.component.css']
})
export class LentedBooksComponent {
  books : Book[] = []
  userEmail!:string

  constructor(
    private booksService: BooksService,
    private authService: AuthService,
    private router:Router
  ){}

  ngOnInit(){
    this.authService.getTokenChange().subscribe((token:string|null) =>{ 
    const decodedToken = this.authService.decodeToken(token);
    this.userEmail = decodedToken.email;
  })
    this.loadBooks();
  }

  loadBooks(){
    this.booksService.getAllLentedBooks(this.userEmail).subscribe({
      next: (res: Book[]) =>{
        this.books = res;
        // console.log('all lented books: ', res);
      }
    })
  }
}
