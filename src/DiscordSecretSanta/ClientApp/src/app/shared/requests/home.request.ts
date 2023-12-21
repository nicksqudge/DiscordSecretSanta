import { ApiRequest } from "../api-request";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

export class HomeRequest implements ApiRequest<HomeResponse> {
  url: string = '/api/home';

  send(httpClient: HttpClient): Observable<HomeResponse> {
    return httpClient.get<HomeResponse>(this.url);
  }
}

export type HomeResponse = Readonly<{
  // If true then the config is okay if not then config detail is populated
  configOk: boolean;

  // Only populated if config is not okay
  configDetail?: ReadonlyArray<HomeConfigResponse>;

  // The details of the user
  user?: HomeUserResponse;

  // The details of an active campaign
  activeCampaign?: HomeCampaignResponse;
}>;

export type HomeConfigResponse = Readonly<{
  key: string;
  healthy: boolean;
  reason: string;
}>;

export type HomeUserResponse = Readonly<{
  name: string;
  isAdmin: boolean;
}>

export type HomeCampaignResponse = Readonly<{
  name: string;
}>;
