import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book.model';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  books : Book[] = []
  searchKeyword=""
  filteredBooks:Book[] = []
  constructor(
    private booksService: BooksService,
    private authService: AuthService,
    private router:Router
  ){}

  ngOnInit(){
    this.loadBooks();
  }

  loadBooks(){
    this.booksService.getAllBooks().subscribe({
      next: (res) =>{
        this.books = res;
        // console.log('all books: ', res);
      }
    })
  }
  filterBooks(){
    return this.books.filter(book =>
      book.name.toLowerCase().includes(this.searchKeyword.trim().toLowerCase()) ||
      book.author.toLowerCase().includes(this.searchKeyword.trim().toLowerCase()) ||
      book.genre.toLowerCase().includes(this.searchKeyword.trim().toLowerCase())
    );
  }

  

}
