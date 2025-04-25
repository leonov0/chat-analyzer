"use client";

import { Loader2 } from "lucide-react";
import { useRouter } from "next/navigation";
import { useTransition } from "react";
import { toast } from "sonner";

import { Button } from "@/components/ui/button";
import { routes } from "@/lib/routes";
import { normalizeAxiosError } from "@/lib/utils";

import { useAuth } from "./auth";

export function LogOutForm() {
  const { logout } = useAuth("auth");

  const [isPending, startTransition] = useTransition();

  const router = useRouter();

  function handleSignOut() {
    startTransition(async () => {
      try {
        await logout();
        router.push(routes.home);
      } catch (e) {
        toast.error(normalizeAxiosError(e));
      }
    });
  }

  return (
    <Button onClick={handleSignOut} disabled={isPending} className="w-full">
      {isPending && <Loader2 className="animate-spin" />}
      Log Out
    </Button>
  );
}
