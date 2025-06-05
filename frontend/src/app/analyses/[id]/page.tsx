"use client";

import { Loader2, Send } from "lucide-react";
import { useParams } from "next/navigation";
import { FormEvent, useTransition } from "react";
import { toast } from "sonner";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import { useAnalysis } from "@/features/analyses";
import { cn, normalizeAxiosError } from "@/lib/utils";

export default function ChatPage() {
  const { id } = useParams();

  const { analysis, isLoading, sendMessage } = useAnalysis(id as string);

  const [isPending, startTransition] = useTransition();

  function handleSend(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    const formData = new FormData(event.currentTarget);

    const input = formData.get("input") as string | null;

    if (input === null || input.trim().length === 0) return;

    event.currentTarget.reset();

    startTransition(async () => {
      try {
        await sendMessage(input);
      } catch (error) {
        toast.error(normalizeAxiosError(error));
      }
    });
  }

  return (
    <div className="grid h-full grid-rows-[auto_1fr_auto] gap-4">
      <h1 className="text-xl font-semibold">{analysis?.name}</h1>

      {isLoading && <Skeleton />}

      <ul className="space-y-2">
        {analysis?.messages.map((message) => (
          <li
            key={message.id}
            className={cn(
              "flex max-w-100 flex-col rounded-md p-2",
              message.type === 1
                ? "bg-secondary text-secondary-foreground ml-auto self-end"
                : "bg-primary text-primary-foreground",
            )}
          >
            {message.content}

            <time className="text-muted-foreground mt-2 text-sm">
              {new Date(message.created_at).toLocaleString()}
            </time>
          </li>
        ))}
      </ul>

      <form className="flex gap-2" onSubmit={handleSend}>
        <Input placeholder="Ask anything" name="input" required />
        <Button size="icon" type="submit" disabled={isPending}>
          {isPending ? <Loader2 className="animate-spin" /> : <Send />}
        </Button>
      </form>
    </div>
  );
}
