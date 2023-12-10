import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor() {
  }
}

export type HomeResponse = Readonly<{
  // If true then the config is okay if not then config detail is populated
  configOk: boolean;

  // Only populated if config is not okay
  configDetail?: ReadonlyArray<ConfigCheck>;

  // Are there any admins setup on the platform
  admins: boolean;

  // The details of the user
  user?: User;

  // The details of an active campaign
  activeCampaign?: Campaign;
}>;

export type ConfigCheck = Readonly<{
  key: string;
  healthy: boolean;
  reason: string;
}>;

export type User = Readonly<{
  name: string;
  isAdmin: boolean;
}>

export type Campaign = Readonly<{
  name: string;
}>;
