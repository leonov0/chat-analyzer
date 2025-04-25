import useSWR from "swr";

import { axiosInstance } from "@/lib/axios";

import { getAnalysisRoute, getSendMessageRoute } from "./api-routes";
import type { Analysis, Message } from "./types";

async function fetchAnalysis(id: string) {
  const { data } = await axiosInstance.get<Analysis>(getAnalysisRoute(id));

  return data;
}

export function useAnalysis(id: string) {
  const {
    data: analysis,
    isLoading,
    mutate,
  } = useSWR(getAnalysisRoute(id), () => fetchAnalysis(id));

  async function sendMessage(message: string) {
    if (analysis === undefined) return;

    const newMessage = {
      id: new Date().toISOString(),
      content: message,
      type: 1,
      created_at: new Date().toISOString(),
      updated_at: new Date().toISOString(),
    } satisfies Message;

    const newAnalysis = {
      ...analysis,
      messages: [...(analysis?.messages || []), newMessage],
    } satisfies Analysis;

    mutate(newAnalysis, false);

    const { data } = await axiosInstance.post<Analysis>(
      getSendMessageRoute(id),
      { message },
    );

    mutate(data, false);
  }

  return {
    analysis,
    isLoading,
    sendMessage,
  };
}
