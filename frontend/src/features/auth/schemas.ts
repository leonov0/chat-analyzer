import { z } from "zod";

export const registerSchema = z
  .object({
    username: z
      .string()
      .min(3)
      .max(20)
      .regex(
        /^[a-zA-Z0-9]+$/,
        "Username can only contain letters and numbers.",
      ),
    email: z.string().email(),
    password: z
      .string()
      .min(8)
      .regex(
        /^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$/,
        "Password must contain at least one uppercase letter, one lowercase letter, and one number.",
      ),
    password_confirmation: z.string(),
  })
  .refine((data) => data.password === data.password_confirmation, {
    path: ["password_confirmation"],
    message: "Passwords don't match",
  });

export const loginSchema = z.object({
  email: z.string().nonempty(),
  password: z.string().nonempty(),
});
