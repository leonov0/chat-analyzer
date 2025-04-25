"use client";

import { useRouter } from "next/navigation";
import { useEffect } from "react";
import useSWR from "swr";

import { axiosInstance } from "@/lib/axios";
import { routes } from "@/lib/routes";

import { APIRoutes } from "./api-routes";
import type { LoginPayload, RegisterPayload, User } from "./types";

async function fetcher() {
  const { data } = await axiosInstance.get<User | null>(APIRoutes.me);

  return data;
}

export function useAuth(middleware?: "guest" | "auth") {
  const { data: user, mutate, isLoading } = useSWR(APIRoutes.me, fetcher);

  const router = useRouter();

  useEffect(() => {
    if (isLoading || middleware === undefined) return;

    if (middleware === "guest" && user) {
      router.push(routes.chats);
    }

    if (middleware === "auth" && !user) {
      router.push(routes.login);
    }
  }, [middleware, router, isLoading, user]);

  async function login(payload: LoginPayload) {
    await axiosInstance.post(APIRoutes.login, payload);

    mutate();
  }

  async function logout() {
    await axiosInstance.post(APIRoutes.logout);

    mutate(null, false);
  }

  async function register(payload: RegisterPayload) {
    await axiosInstance.post(APIRoutes.register, payload);

    mutate();
  }

  return { login, logout, register, user, isLoading };
}
