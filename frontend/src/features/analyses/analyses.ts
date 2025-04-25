"use client";
import { useEffect } from "react";
import useSWR, { mutate as globalMutate } from "swr";

import { useAuth } from "@/features/auth";
import { axiosInstance } from "@/lib/axios";

import { APIRoutes, getAnalysisRoute } from "./api-routes";
import type { Analysis, AnalysisPreview } from "./types";

async function fetcher() {
  const { data } = await axiosInstance.get<AnalysisPreview[]>(
    APIRoutes.analyses,
  );

  return data;
}

export function useAnalyses() {
  const {
    data: analyses,
    mutate,
    isLoading,
  } = useSWR(APIRoutes.analyses, fetcher);

  const { user } = useAuth("auth");

  useEffect(() => {
    mutate();
  }, [mutate, user]);

  async function createAnalysis(file: File): Promise<Analysis> {
    const { data } = await axiosInstance.postForm<Analysis>(
      APIRoutes.analyses,
      { file },
    );

    mutate((prev) => [...(prev || []), data], false);
    globalMutate(getAnalysisRoute(data.id), data, false);

    return data;
  }

  return { analyses, isLoading, createAnalysis };
}
