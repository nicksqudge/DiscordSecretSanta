import { ComponentInterface } from "@theme/component.interface";
import { Color } from "@theme/common-types";

export interface ProgressInterface extends ComponentInterface {
  color?: Color;
  value: number;
  max: number;
}
