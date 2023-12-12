import { ComponentInterface } from "@theme/component.interface";

export interface CardComponentInterface extends ComponentInterface {
  border: boolean;
  mode: CardComponentMode;
}

export type CardComponentMode = "compact" | "normal";
