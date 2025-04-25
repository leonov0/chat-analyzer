import { z } from "zod";

import { loginSchema } from "./schemas";
import { registerSchema } from "./schemas";

export type User = {
  id: string;
  username: string;
  email: string;
};

export type RegisterPayload = z.infer<typeof registerSchema>;

export type LoginPayload = z.infer<typeof loginSchema>;
