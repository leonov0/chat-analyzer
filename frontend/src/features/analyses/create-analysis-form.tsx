"use client";

import { Loader2 } from "lucide-react";
import { useRouter } from "next/navigation";
import { FormEvent, useTransition } from "react";
import { toast } from "sonner";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { getAnalysesRoute } from "@/lib/routes";
import { normalizeAxiosError } from "@/lib/utils";

import { useAnalyses } from "./analyses";

export function CreateAnalysisForm() {
  const router = useRouter();

  const { createAnalysis } = useAnalyses();

  const [isPending, startTransition] = useTransition();

  function onSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const formData = new FormData(event.currentTarget);
    const file = formData.get("file") as File | null;

    if (!file || file.size === 0) {
      toast.error("Please select a file to upload.");
      return;
    }

    if (file.size > 10 * 1024 * 1024) {
      toast.error("File size exceeds the limit of 10MB.");
      return;
    }

    startTransition(async () => {
      try {
        const analysis = await createAnalysis(file);
        router.push(getAnalysesRoute(analysis.id));
      } catch (error) {
        toast.error(normalizeAxiosError(error));
      }
    });
  }

  return (
    <form onSubmit={onSubmit}>
      <Input type="file" name="file" required />
      <Button type="submit" disabled={isPending} className="mt-4 w-full">
        {isPending && <Loader2 className="animate-spin" />}
        Create Analysis
      </Button>
    </form>
  );
}
