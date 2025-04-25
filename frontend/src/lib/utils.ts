import axios from "axios";
import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function normalizeAxiosError(error: unknown): string {
  if (axios.isAxiosError(error)) {
    return error.response?.data.title;
  }

  return "An unexpected error occurred. Please try again later.";
}
