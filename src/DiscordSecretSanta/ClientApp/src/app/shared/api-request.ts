import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

export interface ApiRequest<TResult> {
  url: string;

  send(httpClient: HttpClient): Observable<TResult>;
}